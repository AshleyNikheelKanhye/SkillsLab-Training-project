function validateForm() {
    var trainingName = document.getElementById('TrainingName').value;
    var trainingCapacity = document.getElementById('TrainingCapacity').value;
    var deadlineRegistration = document.getElementById('DeadlineRegistration').value;
    var startingDate = document.getElementById('StartingDate').value;
    var department = document.getElementById('Department').value;
    var trainingDuration = document.getElementById('TrainingDuration').value;

    // Validate TrainingName
    if (trainingName.trim() === '') {

        toastr.error("Training Name cannot be empty.", '', { positionClass: 'toast-bottom-right', backgroundColor: '#4CAF50' });
        return;
    }
    // Validate special characters in TrainingName
    if (!isValidString(trainingName)) {

        toastr.error("Training Name cannot contain special characters.", '', { positionClass: 'toast-bottom-right', backgroundColor: '#4CAF50' });
        return;
    }

    // Validate TrainingCapacity
    if (parseInt(trainingCapacity) <= 0 || isNaN(trainingCapacity)) {

        toastr.error("Training Capacity should be greater than 0.", '', { positionClass: 'toast-bottom-right', backgroundColor: '#4CAF50' });
        return;
    }

    //validate TrainingDuration
    if (parseInt(trainingDuration) <= 0 || isNaN(trainingDuration)) {
        toastr.error("Training Capacity should be greater than 0.", '', { positionClass: 'toast-bottom-right', backgroundColor: '#4CAF50' });
        return;
    }

    // Validate StartingDate and DeadlineRegistration
    var startingDateObj = new Date(startingDate);
    var deadlineRegistrationObj = new Date(deadlineRegistration);
    if (startingDateObj <= deadlineRegistrationObj) {

        toastr.error("Starting Date should be greater than Deadline Registration.", '', { positionClass: 'toast-bottom-right', backgroundColor: '#4CAF50' });
        return;
    }

    // Validate Department
    if (department === '') {

        toastr.error("Please select a Department.", '', { positionClass: 'toast-bottom-right', backgroundColor: '#4CAF50' });
        return;
    }

    // If all validations pass, submit the form
    submitFormData();
}

function isValidString(str) {
    var regex = /^[a-zA-Z0-9\s]*$/;
    return regex.test(str);
}

function submitFormData() {
    var form = document.getElementById('sampleForm');
    // Create FormData object
    var formData = new FormData(form);
    var formDataJson = {};
    //convert FormData object
    formData.forEach((value, key) => {
        // Check if the key already exists in formDataJson
        if (formDataJson[key] !== undefined) {
            // If it exists and is an array, push the new value
            if (Array.isArray(formDataJson[key])) {
                formDataJson[key].push(value);
            } else {
                // If it exists but is not an array, convert it to an array and add the new value
                formDataJson[key] = [formDataJson[key], value];
            }
        } else {
            // If the key doesn't exist, simply add the value
            formDataJson[key] = value;
        }
    });


    //send formdatajson to training controller
    var serverCall = new ServerCall({ url: "/Training/AddTraining", parameters: formDataJson, callMethod: "POST" });
    serverCall.fetchApiCall().then((response) => {
        if (response.result == true) {
            Swal.fire({
                icon: 'success',
                title: 'New Training Inserted',
                text: 'press OK to continue',
                allowOutsideClick: false,
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location = "/Admin/ViewTrainingsView";
                }
            });
        } else {
            toastr.error("could not insert New Training");
        }
    }).catch(error => {
        console.error("error inserting new training");
        toastr.error('could not insert New training');
    });
}



$(document).ready(function () {
    $('nav .nav-container a').removeClass('active');
    $('#adminAddTraining').addClass('active');

    let form = document.querySelector('form');
    form.addEventListener('submit', (e) => {
        e.preventDefault();
        return false;
    });

    function PopulateDropdownListOfDepartments() {
        var serverCall = new ServerCall({ url: "/User/GetDepartments", callMethod: "GET" });
        serverCall.fetchApiCall().then((response) => {
            const departmentDropdown = document.getElementById('Department');
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

    function PopulateCheckboxesListOfPrerequisites() {
        var serverCall = new ServerCall({ url: "/Prerequisite/GetAllPrerequisites", callMethod: "GET" });
        serverCall.fetchApiCall().then((response) => {
            var checkboxGroup = document.querySelector('.checkbox-group');
            if (response && response.listPrerequisites) {
                // Iterate through the listPrerequisites array and create checkboxes
                response.listPrerequisites.forEach((prerequisite) => {
                    // create a div space
                    var checkboxSpan = document.createElement('span');

                    // Create a checkbox element
                    var checkbox = document.createElement('input');
                    checkbox.type = 'checkbox';
                    checkbox.name = 'PrerequisiteList'; // You can set a unique name if needed
                    checkbox.value = prerequisite.PrerequisiteID;
                    checkbox.id = prerequisite.PrerequisiteID;

                    var label = document.createElement('label');
                    label.htmlFor = prerequisite.PrerequisiteID;
                    label.appendChild(document.createTextNode(prerequisite.Details));

                    // Apply styles to align items in the flex container
                    checkboxSpan.style.display = 'flex';
                    checkboxSpan.style.alignItems = 'center';

                    // Append the checkbox and label to the checkboxGroup
                    checkboxGroup.appendChild(checkboxSpan);
                    checkboxSpan.appendChild(checkbox);
                    checkboxSpan.appendChild(label);
                });
            } else {
                console.error('Invalid response format');
            }
        })
            .catch(error => {
                console.error('Error fetching list of prerequisites:', error);
                toastr.error('could not load list of prerequisites');
            });

    }

    //flow
    PopulateDropdownListOfDepartments();
    PopulateCheckboxesListOfPrerequisites();

});

