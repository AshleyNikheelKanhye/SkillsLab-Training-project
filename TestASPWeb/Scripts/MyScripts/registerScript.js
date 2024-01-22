document.addEventListener("DOMContentLoaded", () => {
    let form = document.querySelector('form');
    form.addEventListener('submit', (e) => {
        e.preventDefault();
        return false;
    });

    GetListOfDepartmentsFromDb();
    GetListOfManagersFromDb();
})

function GetListOfDepartmentsFromDb() {
    var serverCall = new ServerCall({url: "/User/GetDepartments",callMethod: "GET"});

    serverCall.fetchApiCall().then(response => {
        const departmentDropdown = document.getElementById('department');
        if (response.listDepartments) {
            response.listDepartments.forEach(department => {
                const option = document.createElement('option');
                option.value = department.DepartmentID;
                option.textContent = department.DepartmentName;
                departmentDropdown.appendChild(option);
            });
        }
        if (response.error) {
            toastr.error('could not load Department list');
        }
        
    })
    .catch(error => {
        console.error('Error fetching departments:', error);
        toastr.error('could not load Departments list');
    });
}

function GetListOfManagersFromDb() {
    var serverCall = new ServerCall({url: "/User/GetManagers",callMethod: "GET"});

    serverCall.fetchApiCall().then(response => {
        const managersDropdown = document.getElementById('managers');
        if (response.listManagers) {
            response.listManagers.forEach(manager => {
                const option = document.createElement('option');
                option.value = manager.UserID;
                var fullName = manager.FirstName +' '+ manager.LastName
                option.textContent = fullName;
                managersDropdown.appendChild(option);
            });
        }
        if (response.error){
            toastr.error('could not load Managers list');
        }
    })
    .catch(error => {
        console.error('Error fetching departments:', error);
        toastr.error('could not load Managers list')
    });
}

function validateForm() {
    var formData = readFormData();
    
    //Frontend Validation
    if (!isValidEmail(formData['Email'])) {
        toastr.error("Please enter a valid email address");
        return;
    }

    if (!isValidText(formData['FirstName'])) {
        toastr.error("Please enter a valid first name");
        return;
    }

    if (!isValidText(formData['LastName'])) {
        toastr.error("Please enter a valid last name");
        return;
    }

    if (!isValidText(formData['NIC'])) {
        toastr.error("Please enter a valid NIC");
        return;
    }

    if (!isValidPhoneNumber(formData['PhoneNo'])) {
        toastr.error("Please enter a valid phone number");
        return;
    }

    if (!isValidPassword(formData['Password'])) {
        toastr.error("Please enter a valid password");
        return;
    }

    if (!isValidSelection(formData['DepartmentID'])) {
        toastr.error("Please select a department");
        return;
    }

    if (!isValidSelection(formData['ManagerID'])) {
        toastr.error("Please select a manager");
        return;
    }
    
    //check if already registered backend
    var inputDetails = {Email: formData['Email'] , NIC:formData['NIC'] , PhoneNo: formData['PhoneNo']}; 
    var serverCall = new ServerCall({ url: "/User/CheckUserExist", parameters: inputDetails, callMethod: "POST" });
    serverCall.fetchApiCall().then(response => {
        if (response.result == true) {
            toastr.error("These credentials have already been used by a user");
            return;
        } else {
            register(formData);
        }
    });
}

function register(formData) {
    var serverCall = new ServerCall({ url: "/User/Register", parameters: formData, callMethod: "POST" });
    serverCall.fetchApiCall().then(response => {
        if (response.result==true) {
            toastr.success("Registered Successfully !");
            window.location = "/Employee/EmployeeView";
        } else {
            toastr.error("Error while registering");
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

// Helper functions for specific validations
function isValidEmail(email) {
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return email.trim() !== "" && emailRegex.test(email);
}

function isValidText(text) {
    return text.trim() !== "";
}

function isValidPhoneNumber(phoneno) {
    var phoneRegex = /^5\d{7}$/;
    return phoneRegex.test(phoneno);
}

function isValidPassword(password) {
    return password.trim() !== "";
}

function isValidSelection(value) {
    return value.trim() !== "";
}
