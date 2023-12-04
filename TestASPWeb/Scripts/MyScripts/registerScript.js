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
    if (!formData['firstName'] && !formData['lastName']) {
        alert('please fill in the names');
        return false;
    }


    //check if already registered backend
    var inputDetails = {}; ///create new view model and pass to api.
    var serverCall = new ServerCall({ url: "/User/CheckUser", parameters:inputDetails, callMethod: "GET" });
    serverCall.fetchApiCall().then(response => {
        if (response.result) {




        } else {
            toastr.error("Unable to check in background");
        }
    })

    

    register();
    return true;
    
}

function register() {
    var formData = readFormData();
    alert("youve been registered");

    console.log(formData);
}


function readFormData() {
    var formData = {};
    formData['firstName'] = document.getElementById("firstName").value;
    formData['lastName'] = document.getElementById("lastName").value;
    formData['email'] = document.getElementById("email").value;
    formData['NIC'] = document.getElementById("NIC").value;
    formData['phoneno'] = document.getElementById("phoneno").value;
    formData['password'] = document.getElementById("password").value;
    formData['department'] = document.getElementById("department").value;
    formData['manager'] = document.getElementById("managers").value;

    return formData;

}










