$(document).ready(function () {
    $('nav .nav-container a').removeClass('active');
    $('#adminTrainings').addClass('active');

    let form = document.querySelector('form');
    form.addEventListener('submit', (e) => {
        e.preventDefault();
        return false;
    });

    //dropdown select options
    $('#mySelect').change(function () {
        var selectionOption = $(this).val();
        $('.content').hide();
        if (selectionOption == "1") { //all trainings
            populateTableAllTrainings().then(() => {
                $('div#allTrainingsDIV').show();
            });
        }
        if (selectionOption == "2") { //upcoming trainings, with a future startingDate
            populateTableUpcomingTrainings().then(() => {
                $('div#upcomingTrainingsDIV').show();
            });
        }
        if (selectionOption == "3") {//processed Trainings , training that has been automatically processed
            populateTableCompletedTrainings().then(() => {
                $('div#completedTrainingsDIV').show();
            });
        }
        if (selectionOption == "4") {//deleted trainings
            populateTableDeletedTrainings().then(() => {
                $('div#deletedTrainingsDIV').show();
            });
        }
    });

    //populate table All trainings
    function populateTableAllTrainings() {
        return new Promise((resolve, reject) => {
            var URL = "/Training/getAll";
            getAJAXCall(URL).then((response) => {
                if ($.fn.DataTable.isDataTable('#AllTrainingTable')) {
                    $('#AllTrainingTable').DataTable().clear().destroy();
                }
                $('#AllTrainingTable').DataTable({
                    data: response,
                    columns: [
                        { data: 'TrainingName' },
                        { data: 'DepartmentName' },
                        {
                            data: 'ClosingDate',
                            render: function (data, type, row) {
                                return formatDateTime(data);
                            }
                        },
                        {
                            data: 'TrainingStartDate',
                            render: function (data, type, row) {
                                return formatDateTime(data);
                            }
                        },
                        {
                            data: 'TrainingID',
                            render: function (data, type, row) {
                                return '<button id="viewEmployeesBtn" value="' + data + '">View Employees</button>'
                            }
                        },
                        {
                            data: 'TrainingID',
                            render: function (data, type, row) {
                                return '<button id="viewTraningPrerequisiteBtn"value="' + data + '">View Prerequisites</button>'
                            }
                        },
                    ],
                    ordering: false,
                    createdRow: function (row, data, dataIndex) {
                        var closingDate = new Date(parseInt(data.ClosingDate.substr(6)));
                        var currentDate = new Date();

                        if (closingDate < currentDate) {
                            $(row).css('background-color', '#e6ffee');
                        }
                    }
                });
                $('div#emptyListHeaderDiv').empty().hide();
                $('div#AllTrainingTableDIV').show();
                resolve();
            }).catch((error) => {
                console.error(error);
                toastr.error("Error on loading Trainings table");
                reject();
            });
        });
    }


    //populate table upcoming Trainings
    function populateTableUpcomingTrainings() {
        return new Promise((resolve, reject) => {
            var URL = "/Training/getUpcomings";
            getAJAXCall(URL).then((response) => {
                if (response) {
                    var tableBody = $('#UpcomingTrainingTable tbody');
                    tableBody.empty();
                    if (response.length === 0) {
                        $('div#UpcomingTrainingTable').hide();
                        $('div#upcomingTrainingEmptyListHeader').html("<h2>Seems there are no upcoming trainings</h2>");
                        resolve();
                    }
                    else {
                        $.each(response, function (index, training) {
                            var row = '<tr>' +
                                '<td>' + training.TrainingName + '</td>' +
                                '<td>' + training.NumberOfEnrollments + '</td>' +
                                '<td>' + training.Capacity + '</td>' +
                                '<td>' + formatDateTime(training.ClosingDate) + '</td>' +
                                '<td>' + formatDateTime(training.TrainingStartDate) + '</td>' +
                                '<td>' + training.Duration + '</td>' +
                                '<td><button id="viewEmployeesBtn" value="' + training.TrainingID + '">Application List</button></td>' +
                                '<td><button id="viewTraningPrerequisiteBtn" value="' + training.TrainingID + '">View Prerequisites</button></td>' +
                                '<td>' + returnStatus(training.IsAutomaticProcessed) + '</td>';
                            if (training.IsAutomaticProcessed == false) {
                                row = row + '<td><button id="UpdateTrainingBtn" value="' + training.TrainingID + '">Update</button>';
                                if (training.NumberOfEnrollments == 0) {
                                    row = row + '<button id="DeleteTrainingBtn" value="' + training.TrainingID + '">Delete</button></td>';
                                } else {
                                    row = row + '</td>'
                                }
                            }
                            row = row + '</tr>';
                            tableBody.append(row);
                        });
                        $('div#upcomingTrainingEmptyListHeader').empty().hide();
                        $('div#UpcomingTrainingTableDiv').show();
                        resolve();
                    }
                }
                else {
                    toastr.error("Error in backend");
                    reject();
                }
            }).catch((error) => {
                console.error(error);
                toastr.error('error loading in backend');
                reject();
            });
        })
    }

    //populate table completed Trainings(AKA processed Training)
    function populateTableCompletedTrainings() {
        return new Promise((resolve, reject) => {
            var URL = "/Training/GetCompletedTrainings";
            getAJAXCall(URL).then((response) => {
                if (response) {
                    var tableBody = $('#completedTrainingTable tbody');
                    tableBody.empty();
                    if (response.length === 0) {
                        $("#completedTrainingTableDiv").hide();
                        $("#completedTrainingEmptyListHeader").html("<h2>Looks like there are no Training that has been completed (Automtic processed)</h2>").show();
                        resolve();
                    } else {
                        $.each(response, function (index, training) {
                            var row = '<tr>' +
                                '<td>' + training.TrainingName + '</td>' +
                                '<td>' + formatDateTime(training.TrainingStartDate) + '</td>' +
                                '<td>' + training.Duration + '</td>' +
                                '<td><button id="viewMoreDetailsBtn" value="' + training.TrainingID + '">View Details</button></td>' +
                                '</tr>';
                            tableBody.append(row);
                        });
                        $('div#completedTrainingEmptyListHeader').empty().hide();
                        $('div#completedTrainingTableDiv').show();
                        resolve();
                    }
                }
                else {
                    toastr.error("error");
                }
            }).catch((error) => {
                console.error(error);
                toastr.error("error on loading completed Trainigns");
                reject();
            });
        });
    }


    //populate table Deleted Trainings
    function populateTableDeletedTrainings() {
        return new Promise((resolve, reject) => {
            var URL = "/Training/GetDeletedTrainings";
            getAJAXCall(URL).then((response) => {
                if (response) {
                    var tableBody = $('#deletedTrainingTable tbody');
                    tableBody.empty();
                    if (response.length === 0) {
                        $('#deletedTrainingTableDiv').hide();
                        $('#deletedTrainingEmptyListHeader').html("<h2>There are not Trainings that have been deleted</h2>").show();
                        resolve();
                    } else {
                        $.each(response, function (index, training) {
                            var row = '<tr>' +
                                '<td>' + training.TrainingName + '</td>' +
                                '<td>' + training.Capacity + '</td>' +
                                '</tr>';
                            tableBody.append(row);
                        });
                        $('div#deletedTrainingEmptyListHeader').empty().hide();
                        $('#deletedTrainingTableDiv').show();
                        resolve();
                    }
                } else {
                    toastr.error("error");
                    reject();
                }
            }).catch((error) => {
                console.error(error);
                toastr.error("error on loading Deleted trainings");
                reject();
            });
        });
    }

    //When clicking on view employees btn
    $('.requestsTable tbody').on("click", "#viewEmployeesBtn", function () {
        var row = $(this).closest("tr");
        var selectedTrainingID = $(this).val();
        var selectedTrainingName = row.find("td:eq(0)").text();
        $('div#popupHeaderDiv').empty();
        $('div#popupHeaderDiv').html("<h3>Employees that has applied for " + selectedTrainingName + "</h3>");
        var trainingIDObj = { trainingID: selectedTrainingID };
        var serverCall = new ServerCall({ url: "/Enrollment/GetEmployeesAppliedForTraining", parameters: trainingIDObj, callMethod: "POST" });
        serverCall.fetchApiCall().then((response) => {
            if (response) {
                $('.content').hide();
                var tableBody = $('#employeeTable tbody');
                tableBody.empty();
                if (response.length === 0) {
                    $('div#employeesTableDiv').hide();
                    $('div#popupHeaderDiv').append("<br><h1>Looks Like nobody has applied for this training</h1>").fadeIn();
                    $('div#employeesInTrainingPopUp').show();
                }
                else {
                    $('div#popupHeaderDiv').append("<br><h4> Total of " + response.length + " applications received</h4>").show();
                    $.each(response, function (index, employee) {
                        var row = '<tr>' +
                            '<td>' + employee.FirstName + ' ' + employee.LastName + '</td>' +
                            '<td>' + employee.Email + '</td>' +
                            '<td>' + formatDateTime(employee.DateRegistered) + '</td>' +
                            '<td>' + employee.ManagerStatus + '</td>' +
                            '<td>' + employee.FinalStatus + '</td>' +
                            '</tr>';
                        tableBody.append(row);
                    });
                    $('div#popupHeaderDiv').show();
                    $('div#employeesTableDiv').show();
                    $('div#employeesInTrainingPopUp').show();
                }
            }
            else {
                toastr.error("Error loading Employees");
            }
        });
    });

    //when clicking on view Training Prerequisite
    $('.requestsTable tbody').on("click", "#viewTraningPrerequisiteBtn", function () {
        var URL = "/Training/GetPrerequisites";
        var row = $(this).closest("tr");
        var trainingID = $(this).val();
        var selectedTrainingName = row.find("td:eq(0)").text();
        var trainingobj = { trainingID: trainingID }
        var serverCall = new ServerCall({ url: URL, parameters: trainingobj, callMethod: "POST" });
        serverCall.fetchApiCall().then((response) => {
            if (response) {
                $('.content').hide();
                $('div#popupTrainingPrerequisiteContentDiv').empty();
                if (response.result.length === 0) {
                    $('#headerTrainingPrerequisite').html("<h2>Looks like this training does not have any prerequisites</h2>");
                    $('#popupViewTrainingPrerequisiteDiv').show();
                }
                else {
                    $('#headerTrainingPrerequisite').html("<h2>Prerequisites For " + selectedTrainingName + "</h2>");
                    response.result.forEach(function (p) {
                        var paragraph = document.createElement('h6');
                        paragraph.textContent = ' - ' + p.Details;
                        $('div#popupTrainingPrerequisiteContentDiv').append(paragraph);
                    });
                    $('#popupViewTrainingPrerequisiteDiv').show();
                }
            }
            else {
                toastr.error("could not load prerequisites");
            }
        });
    });

    //when clicking on view selected employees Details btn
    $('.requestsTable tbody').on('click', '#viewMoreDetailsBtn', function () {
        var tableBody = $('#selectedEmployeeTable tbody');
        var selectedTrainingID = $(this).val();
        var URL = "/Training/GetSelectedEmployees";
        var row = $(this).closest("tr");
        var selectedTrainingName = row.find("td:eq(0)").text();
        var selectedTrainingStartingDate = row.find("td:eq(1)").text();
        var selectedTrainingDuration = row.find("td:eq(2)").text();
        var trainingIdObj = { trainingId: selectedTrainingID };
        var serverCall = new ServerCall({ url: URL, parameters: trainingIdObj, callMethod: "POST" });
        serverCall.fetchApiCall().then((response) => {
            if (response) {
                $('.content').hide();
                tableBody.empty();
                $('#trainingDetailsHeader').html("<h2>List of accepted employees for " + selectedTrainingName + "<h2> <h5>Starting Date : " + selectedTrainingStartingDate + "</h5><h5>Duration(days) : " + selectedTrainingDuration + "</h5>").show();
                $.each(response, function (index, employee) {
                    var row = '<tr>' +
                        '<td>' + employee.FirstName + ' ' + employee.LastName + '</td>' +
                        '<td>' + employee.Email + '</td>' +
                        '<td>' + employee.PhoneNo + '</td>' +
                        '<td>' + employee.NIC + '</td>' +
                        '<td>' + employee.ManagerFirstName + ' ' + employee.ManagerLastName + '</td>' +
                        '<td>' + employee.ManagerEmail + '</td>' +
                        '<tr>';
                    tableBody.append(row);
                });
                $('button#exportToCvsBtn').attr("value", selectedTrainingID).attr("name", selectedTrainingName);
                $('div#selectedEmployeesTableDiv').show();
                $('div#trainingDetailsDiv').show();
            } else {
                toastr.error("Error");
            }
        });
    });


    //when clicking download csv
    $(document).on("click", "#exportToCvsBtn", function () {
        var URL = "/Training/GetSelectedEmployees";
        var selectedTrainingID = $(this).val();
        var selectedTrainingName = $(this).attr('name');
        var trainingIdobj = { trainingId: selectedTrainingID };

        var serverCall = new ServerCall({ url: URL, parameters: trainingIdobj, callMethod: "POST" });
        serverCall.fetchApiCall().then((response) => {
            // Convert JSON to CSV
            const csvData = convertJSONToCSV(response);

            // Create a Blob object containing the CSV data
            const blob = new Blob([csvData], { type: "text/csv" });

            // Create a download link
            const link = document.createElement("a");
            link.href = window.URL.createObjectURL(blob);
            link.download = "Selected Employees of " + selectedTrainingName + ".csv";

            // Append the link to the document
            document.body.appendChild(link);

            // Trigger the click event on the link
            link.click();

            // Remove the link from the document
            document.body.removeChild(link);
        });
    });

    //when clicking on Update Training
    $('.requestsTable tbody').on('click', "#UpdateTrainingBtn", function () {
        var URL = "/Training/GetTrainingToUpdateDetails";
        var selectedTrainingID = $(this).val();
        var trainingIDObj = { trainingID: selectedTrainingID };
        var serverCall = new ServerCall({ url: URL, parameters: trainingIDObj, callMethod: "POST" });
        serverCall.fetchApiCall().then((response) => {
            if (response) {
                $('.content').hide();
                $('#updatePopupDiv').show();
                $('#TrainingName').val(response.TrainingName);
                $('#TrainingCapacity').val(response.Capacity);
                document.getElementById('DeadlineRegistration').value = convertTimestampToString(response.ClosingDate);
                document.getElementById('StartingDate').value = convertTimestampToString(response.TrainingStartDate);
                $('#TrainingDuration').val(response.Duration);
                $('#TrainingDescription').val(response.Description);
                $('#updateBtn').attr("value", selectedTrainingID);
            } else {
                toastr.error("Error loading Training Update Page");
            }
        });
    });

    //when clicking on delete training
    $('.requestsTable tbody').on('click', '#DeleteTrainingBtn', function () {
        var selectedTrainingID = $(this).val();
        Swal.fire({
            title: 'Are you sure?',
            text: 'You won\'t be able to revert this!',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'No, cancel!',
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                var URL = "/Training/Delete";
                var trainingIDobj = { trainingID: selectedTrainingID };
                var serverCall = new ServerCall({ url: URL, parameters: trainingIDobj, callMethod: "POST" });
                serverCall.fetchApiCall().then((response) => {
                    if (response && response === true) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Training Deleted',
                            text: 'press OK to continue',
                            allowOutsideClick: false,
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location = "/Admin/ViewTrainingsView";
                            }
                        });
                    } else {
                        Swal.fire('Error!', 'Something Went Wrong, Training could not be deleted', 'error');
                    }
                }).catch((error) => {
                    toastr.error("Error, Training Could not be deleted");
                });
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.close();
            }
        });
    });

    //close button for update training
    $(document).on("click", "button#cancelUpdateBtn", function () {
        $('.content').hide();
        $("div#upcomingTrainingsDIV").show();
    });

    //close button for popup selected employees
    $(document).on("click", "button#closeBtnSelectedEmployees", function () {

        $('.content').hide();
        $('div#completedTrainingsDIV').show();
    });

    //close button for popup employees
    $(document).on("click", "button#closePopUpEmployeeBtn", function () {
        $('div#employeesInTrainingPopUp').hide();
        var selectedOption = $('#mySelect').val();
        if (selectedOption == "1") { $('div#allTrainingsDIV').show(); }
        if (selectedOption == "2") { $('div#upcomingTrainingsDIV').show(); }
        if (selectedOption == "3") { $('div#completedTrainingsDIV').show(); }
        if (selectedOption == "4") { $('div#deletedTrainingsDIV').show(); }
    });

    //close button for popup prerequisite
    $(document).on("click", "button#closePopupTrainingPrerequisiteBtn", function () {
        $('div#popupViewTrainingPrerequisiteDiv').hide();
        var selectedOption = $('#mySelect').val();
        if (selectedOption == "1") { $('div#allTrainingsDIV').show(); }
        if (selectedOption == "2") { $('div#upcomingTrainingsDIV').show(); }
        if (selectedOption == "3") { $('div#completedTrainingsDIV').show(); }
        if (selectedOption == "4") { $('div#deletedTrainingsDIV').show(); }
    });

    function getAJAXCall(URL) {
        return new Promise((resolve, reject) => {
            $.ajax({
                type: "GET",
                url: URL,
                data: null,
                dataType: "json",
                success: function (data) {
                    resolve(data);
                },
                error: function (data) {
                    reject(data);
                }
            })
        });
    }
    function formatDateTime(date) {
        if (date) {
            var parsedDate = new Date(parseInt(date.substr(6)));
            return parsedDate.toLocaleDateString() + ' ' + parsedDate.toLocaleTimeString();
        } else {
            return 'No date';
        }
    }

    function returnStatus(booleanIsAutomaticProcessed) {
        if (booleanIsAutomaticProcessed == true) {
            return 'DONE';
        } else {
            return 'PENDING';
        }
    }

    function convertTimestampToString(timestamp) {
        let dateTimeObject = new Date(Number((timestamp).match(/\d+/)[0]));
        return dateTimeObject.toISOString().slice(0, 19).replace("T", " ");
    }


    //modal
    $(document).on("click", "#trainingInfoBtn", function () {
        document.getElementById('id01').style.display = 'block';
    });


    //flow
    $('#mySelect').trigger('change');

});

function isValidString(str) {
    var regex = /^[a-zA-Z0-9\s]*$/;
    return regex.test(str);
}

function validateForm() {
    var trainingName = document.getElementById('TrainingName').value;
    var trainingCapacity = document.getElementById('TrainingCapacity').value;
    var deadlineRegistration = document.getElementById('DeadlineRegistration').value;
    var startingDate = document.getElementById('StartingDate').value;
    var trainingDuration = document.getElementById('TrainingDuration').value;
    var TrainingDescription = document.getElementById('TrainingDescription').value;


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

    submitFormData();
}

function submitFormData() {
    var form = document.getElementById('sampleForm');
    var formData = new FormData(form);
    var formDataJson = {};
    formData.forEach((value, key) => {
        if (formDataJson[key] !== undefined) {
            if (Array.isArray(formDataJson[key])) {
                formDataJson[key].push(value);
            } else {
                formDataJson[key] = [formDataJson[key], value];
            }
        } else {
            formDataJson[key] = value;
        }
    });

    //add trainingID to formdataJson
    var selectedTrainingID = document.getElementById('updateBtn').value;
    formDataJson['TrainingID'] = selectedTrainingID;

    //send formdataJson to training Controller
    var serverCall = new ServerCall({ url: "/Training/UpdateTraining", parameters: formDataJson, callMethod: "POST" });
    serverCall.fetchApiCall().then((response) => {
        if (response.result == true) {
            Swal.fire({
                icon: 'success',
                title: 'Training Updated',
                text: 'press OK to continue',
                allowOutsideClick: false,
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location = "/Admin/ViewTrainingsView";
                }
            });
        } else {
            toastr.error("could not update Training");
        }
    }).catch(error => {
        console.error("error updating training");
        toastr.error('could not update training');
    });

}

function convertJSONToCSV(jsonData) {
    if (!Array.isArray(jsonData) || jsonData.length === 0) {
        console.error("JSON data is not an array or is empty.");
        return '';
    }

    const csvRows = [];
    const headers = Object.keys(jsonData[0]);

    if (headers.length === 0) {
        console.error("JSON data does not contain any objects with properties.");
        return '';
    }

    // Add headers to CSV
    csvRows.push(headers.join(','));

    // Add data rows to CSV
    jsonData.forEach((item) => {
        if (typeof item !== 'object' || item === null) {
            console.error("Invalid data found. Each element in the JSON array should be an object.");
            return;
        }
        const values = headers.map(header => {
            // Convert null values to empty strings
            return (item[header] === null) ? '' : item[header];
        });
        csvRows.push(values.join(','));
    });
    // Join rows with newline character
    return csvRows.join('\n');
}
