document.addEventListener("DOMContentLoaded", () => {
    let form = document.querySelector('form');
    form.addEventListener('submit', (e) => {
        e.preventDefault();
        return false;
    });
})

document.addEventListener("DOMContentLoaded", function () {

    GetListOfDepartmentsFromDb();
    GetListOfManagersFromDb();
});


function GetListOfDepartmentsFromDb() {
    var serverCall = new ServerCall({url: "/User/GetDepartments",callMethod: "GET"});

    serverCall.fetchApiCall().then(response => {
        alert(JSON.stringify(response));
        const departmentDropdown = document.getElementById('department');

        response.listDepartments.forEach(department => {
            const option = document.createElement('option');
            option.value = department.DepartmentID;
            option.textContent = department.DepartmentName;
            departmentDropdown.appendChild(option);
        });
    })
        .catch(error => console.error('Error fetching departments:', error));
}


function GetListOfManagersFromDb() {
    var serverCall = new ServerCall({url: "/User/GetManagers",callMethod: "GET"});

    serverCall.fetchApiCall().then(response => {
        alert(JSON.stringify(response));
        const managersDropdown = document.getElementById('managers');

        response.listManagers.forEach(manager => {
            const option = document.createElement('option');
            option.value = manager.UserID;
            var fullName = manager.FirstName +' '+ manager.LastName
            option.textContent = fullName;
            managersDropdown.appendChild(option);
        });
    })
        .catch(error => console.error('Error fetching list of managers:', error));
}


function validateForm() {
    var formData = readFormData();

    //front end validations 
    if (!formData['FirstName'] && !formData['LastName']) {
        alert('please fill in the names');
        return;
    }
    
    //check if already registered backend
    var inputDetails = {Email: formData['Email'] , NIC:formData['NIC'] , PhoneNo: formData['PhoneNo']}; 
    var serverCall = new ServerCall({ url: "/User/CheckUserExist", parameters: inputDetails, callMethod: "POST" });
    serverCall.fetchApiCall().then(response => {
        if (response.result == true) {
            alert("youre already registered");
            toastr.error("These credentials have already been used by a user");
            return;
        } else {
            register(formData);
        }
    });
}

function register(formData) {
  
    //alert("youve been registered");
    //console.log(formData);
    formData['Role'] = "employee";
    var serverCall = new ServerCall({ url: "/User/Register", parameters: formData, callMethod: "POST" });
    serverCall.fetchApiCall().then(response => {
        if (response.result) {
            toastr.success("Registered Successfully !");
            window.location = "/Home/Index";
        } else {
            toastr.error("error while registering");
        }
    });



}

function readFormData() {
    var formData = {};
    formData['FirstName'] = document.getElementById("firstName").value;
    formData['LastName'] = document.getElementById("lastName").value;
    formData['Email'] = document.getElementById("email").value;
    formData['NIC'] = document.getElementById("NIC").value;
    formData['PhoneNo'] = document.getElementById("phoneno").value;
    formData['Password'] = document.getElementById("password").value;
    formData['DepartmentID'] = document.getElementById("department").value;
    formData['ManagerID'] = document.getElementById("managers").value;
    return formData;
}










