$(document).ready(function () {
    $('nav .nav-container a').removeClass('active');
    $('#approveRequests').addClass('active');

    //dropdown to select options
    $('#mySelect').change(function () {
        var selectedOption = $(this).val();
        $('.content').hide();
        if (selectedOption == "1") {
            populateTablePendingRequest().then(() => {
                $('div#pendingRequestDIV').show();
            });
        }
        if (selectedOption == "2") {
            populateTableApprovedOrDisapprovedRequest("Approved").then(() => {
                $('div#approvedRequestDIV').show();
            });
        }
        if (selectedOption == "3") {
            populateTableApprovedOrDisapprovedRequest("Disapproved").then(() => {
                $('div#declinedRequestDIV').show();
            });
        }
    });

    //function to populate Approved requests
    function populateTableApprovedOrDisapprovedRequest(Choice) {
        return new Promise((resolve, reject) => {
            var URL = "/Enrollment/GetManagerApproveAndDisapproved";
            var choiceObj = { Choice: Choice };
            var serverCall = new ServerCall({ url: URL, parameters: choiceObj, callMethod: "POST" });
            serverCall.fetchApiCall().then((response) => {
                if (response) {
                    if (Choice == "Approved") {
                        var tableBody = $('#approvedRequestTable tbody');
                    } else {
                        var tableBody = $('#declinedRequestTable tbody');
                    }
                    tableBody.empty();
                    if (response.length === 0) {
                        if (Choice == "Approved") {
                            $('div#approvedTableDiv').hide();
                            $('div#approvedEmptyListDiv').html("<h2>You have not approved any requests yet</h2>");
                            $('div#approvedEmptyListDiv').show();
                        }
                        else {
                            $('div#declinedTableDiv').hide();
                            $('div#declinedEmptyListDiv').html("<h2>You have not decline any requests yet</h2>");
                            $('div#declinedEmptyListDiv').show();
                        }
                        resolve();
                    }
                    else {
                        $.each(response, function (index, enrollment) {
                            if (pastDate(enrollment.ClosingDate) == true) {
                                var row = '<tr class="past">';
                            } else {
                                var row = '<tr class="future">';
                            }
                            row= row  +
                                '<td>' + enrollment.FirstName + ' ' + enrollment.LastName + '</td>' +
                                '<td>' + enrollment.Email + '</td>' +
                                '<td>' + enrollment.TrainingName + '</td>' +
                                '<td>' + formatDateTime(enrollment.ClosingDate) + '</td>' +
                                '<td>' + formatDateTime(enrollment.TrainingStartDate) + '</td>' +
                                '<td>' + formatDateTime(enrollment.DateRegistered) + '</td>' +
                                '<td>' + enrollment.ManagerStatus + '</td>' +
                                '<td>' + enrollment.FinalStatus + '</td>' +
                                '</tr>';
                            tableBody.append(row);
                        })
                        if (Choice == "Approved") {
                            $('approvedEmptyListDiv').hide();
                            $('div#approvedTableDiv').show();
                        } else {
                            $('div#declinedEmptyListDiv').hide();
                            $('div#declinedTableDiv').show();
                        }
                        resolve();
                    }
                }
                else {
                    toast.error("error in backend");
                    reject();
                }
            }).catch((error) => {
                console.error(error);
                toastr.error("error cannot load data");
                reject();
            })
        });
    }



    //function to populate pending requests
    function populateTablePendingRequest() {
        return new Promise((resolve, reject) => {
            var URL = "/Enrollment/GetPendingEnrollments";
            getAJAXCall(URL).then((response) => {
                if (response) {
                    var tableBody = $('#pendingRequestTable tbody');
                    tableBody.empty();
                    if (response.length === 0) {
                        $('div#pendingTableDIV').hide();
                        $('div#EmptyListHeaderDiv').html("<h2>There are No Pending Training Applications for you</h2>");
                        $('div#EmptyListHeaderDiv').show();
                        resolve();
                    }
                    else {
                        $.each(response, function (index, enrollment) {
                            if (pastDate(enrollment.ClosingDate) == true) {
                                var row = '<tr class="past">';
                            } else {
                                var row = '<tr class="future">';
                            }
                            row = row + 
                                '<td>' + enrollment.FirstName + ' ' + enrollment.LastName + '</td>' +
                                '<td>' + enrollment.Email + '</td>' +
                                '<td>' + enrollment.TrainingName + '</td>' +
                                '<td>' + formatDateTime(enrollment.ClosingDate) + '</td>' +
                                '<td>' + formatDateTime(enrollment.TrainingStartDate) + '</td>' +
                                '<td>' + formatDateTime(enrollment.DateRegistered) + '</td>' +
                                '<td><button id="actionBtn" value="' + enrollment.EnrollmentID + '" >Accept/Reject</button></td>' +
                                '</tr>';
                            tableBody.append(row);
                        });
                        $('div#EmptyListHeaderDiv').hide();
                        $('div#pendingTableDIV').show();
                        resolve();
                    }
                }
                else {
                    toast.error("error in backend");
                    reject();
                }
            }).catch((error) => {
                console.error(error);
                toastr.error("error cannot load data");
                reject();
            })
        });
    }

    //function to be triggered when clicking on Accept/Reject button
    $('#pendingRequestTable tbody').on("click", "#actionBtn", function () {
        var row = $(this).closest("tr");
        var selectedEnrollmentID = $(this).val();
        var selectedEmployeeName = row.find("td:eq(0)").text();
        var selectedEmployeeeEmail = row.find("td:eq(1)").text();
        var selectedTrainingName = row.find("td:eq(2)").text();
        var selectedTrainingDeadline = row.find("td:eq(3)").text();
        var selectedTrainingStartDate = row.find("td:eq(4)").text();
        var selectedDateRegistered = row.find("td:eq(5)").text();

        var enrollmentHeaderDetails = $('div#TakeActionContentDetails');
        var enrollmentTableBody = $('#EmployeeQualificationTable tbody');
        enrollmentTableBody.empty();

        //populate enrollment header details
        var bodyText = "<h6>Employee Full Name : " + selectedEmployeeName + " </h6>" +
            "<h6>Employee Email : " + selectedEmployeeeEmail + "</h6>" +
            "<h6>Training Name : " + selectedTrainingName + "</h6>" +
            "<h6>Deadline Application for Training : " + selectedTrainingDeadline + "</h6>" +
            "<h6>Training starts at : " + selectedTrainingStartDate + "</h6>" +
            "<h6>Date Employee Registered for Training :" + selectedDateRegistered + " </h6>";

        enrollmentHeaderDetails.html(bodyText);

        var URL = "/Prerequisite/GetUserPrerequisiteForEnrollment";
        var enrollmentObj = { enrollmentID: selectedEnrollmentID };
        var serverCall = new ServerCall({ url: URL, parameters: enrollmentObj, callMethod: "POST" });
        serverCall.fetchApiCall().then((response) => {
            if (response.list) {
                if (response.list.length > 0) {
                    $.each(response.list, function (index, qualification) {
                        var row = '<tr>' +
                            '<td>' + qualification.Details + '</td>' +
                            '<td>' + qualification.FileName + '</td>' +
                            '<td> <a href="/Prerequisite/DownloadQualification?userID=' + qualification.UserID + '&prerequisiteID=' + qualification.PrerequisiteID + '" target="_blank">Download File </a></td>'
                            + '</tr>';
                        enrollmentTableBody.append(row);
                    });
                    $('#qualificationTableDIV').show();
                } else {
                    $('#qualificationTableDIV').hide();
                    enrollmentHeaderDetails.append("<br><h2>This Training does not require any Qualifications from the applicant</h2><br>");
                }
                //show and hide all appropriate divs
                $('#TakeActionHeader').html(selectedEmployeeName + " Application for Training : " + selectedTrainingName);
                $('button#ManagerApproveBtn').attr("value", selectedEnrollmentID);
                $('button#ManagerDeclineBtn').attr("value", selectedEnrollmentID);
                $('#pendingTableDIV').hide();
                $('#TakeActionPopUpDiv').show();
            }
            else {
                toastr.error("error loading page");
            }
        });

    });

    //textArea for declining training
    function getTextAreaValue() {
        return new Promise((resolve, reject) => {
            Swal.fire({
                title: 'Please enter your reason for Declining this application',
                input: 'textarea',
                inputPlaceholder: 'Type your text here...',
                showCancelButton: true,
                confirmButtonText: 'Submit',
                cancelButtonText: 'Cancel',
                inputValidator: (value) => {
                    if (!value) {
                        return 'You cannot provide an empty reason';
                    }
                }
            }).then((result) => {
                if (result.isConfirmed) {
                    var message = result.value;
                    resolve(message);
                }
            });
        });
    }

    //function disapprove button is clicked
    $(document).on("click", "button#ManagerDeclineBtn", function () {
        var selectedEnrollmentID = $(this).val();
        var URL = "/Enrollment/ManagerUpdatesEnrollment";
        getTextAreaValue().then((message) => {
            var enrollmentObj = { enrollmentID: selectedEnrollmentID, ManagerResult: "Disapproved", DisapproveMessage: message };
            var serverCall = new ServerCall({ url: URL, parameters: enrollmentObj, callMethod: "POST" });
            //show spinner while waiting for backendCall
            $('#pendingRequestDIV').hide();
            var spinnerContainer = document.getElementById('spinner-container');
            spinnerContainer.innerHTML = '<div class="spinner"></div>';
            serverCall.fetchApiCall().then((response) => {
                spinnerContainer.innerHTML = '';
                if (response.result == true) {
                    Swal.fire({
                        icon: 'success',
                        title: 'You have Disapproved the Training Request',
                        text: 'Press Ok to continue',
                        allowOutsideClick: false,
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location = "/Manager/ApproveRequestView";
                        }
                    })
                } else {
                    spinnerContainer.innerHTML = '';
                    $('#pendingRequestDIV').show();
                    toastr.error("Error , could not accept this application")
                }
            }).catch((error) => {
                spinnerContainer.innerHTML = '';
                $('#pendingRequestDIV').show();
                toastr.error("an error occured");
            });
        });
    });

    //function approve button is clicked
    $(document).on("click", "button#ManagerApproveBtn", function () {
        var selectedEnrollmentID = $(this).val();
        var URL = "/Enrollment/ManagerUpdatesEnrollment";
        var enrollmentObj = { enrollmentID: selectedEnrollmentID, ManagerResult: "Approved", DisapproveMessage: "" };
        var serverCall = new ServerCall({ url: URL, parameters: enrollmentObj, callMethod: "POST" });
        //show spinner while waiting for backendCall
        $('#pendingRequestDIV').hide();
        var spinnerContainer = document.getElementById('spinner-container');
        spinnerContainer.innerHTML = '<div class="spinner"></div>';
        serverCall.fetchApiCall().then((response) => {
            spinnerContainer.innerHTML = '';
            if (response.result == true) {
                Swal.fire({
                    icon: 'success',
                    title: 'You have Approved the Training Request',
                    text: 'Press Ok to continue',
                    allowOutsideClick: false,
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location = "/Manager/ApproveRequestView";
                    }
                })
            } else {
                spinnerContainer.innerHTML = '';
                $('#pendingRequestDIV').show();
                toastr.error("Error , could not accept this application")
            }
        }).catch((error) => {
            spinnerContainer.innerHTML = '';
            $('#pendingRequestDIV').show();
            toastr.error("an error occured");
        });
    });


    //go back button is clicked
    $(document).on("click", "button#GobackBtn", function () {
        //clear appropriate divs in popup and value in apply /decline button
        $("#TakeActionHeader").empty();
        $('#EmployeeQualificationTable tbody').empty();
        $('button#ManagerApproveBtn').attr("value", "");
        $('button#ManagerDeclineBtn').attr("value", "");
        $('#TakeActionPopUpDiv').hide();
        $('#pendingTableDIV').show();
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

    function pastDate(date) {
        var closingDate = new Date(parseInt(date.substr(6)));
        var currentDate = new Date();

        if (closingDate < currentDate) {
            return true;
        }
        else {
            return false;
        }
    }

    //flow
    $('#mySelect').trigger('change'); //at start to display pending requests

});