﻿$(document).ready(function () {
    $('nav .nav-container a').removeClass('active');
    $('#adminAutomaticProcess').addClass('active');


    //function to populate table trainings
    function populateTableTrainings() {
        return new Promise((resolve, reject) => {
            var URL = "/Training/GetUnprocessedTrainings";
            getAJAXCall(URL).then((response) => {
                if (response) {
                    var tableBody = $('#trainingsToBeProcessedTable tbody');
                    tableBody.empty();
                    if (response.length === 0) {
                        $('div#trainingsToBeProcessedTableDiv').hide();
                        $('div#emptyListHeader').html("<h2>Looks like there are no trainings that need to be processed</h2>").show();
                        resolve();
                    }
                    else {
                        $.each(response, function (index, training) {
                            var row = '<tr>' +
                                '<td>' + training.TrainingName + '</td>' +
                                '<td>' + formatDateTime(training.ClosingDate) + '</td>' +
                                '<td>' + formatDateTime(training.TrainingStartDate) + '</td>' +
                                '<td>' + training.Capacity + '</td>' +
                                '<td>' + training.DepartmentName + '</td>' +
                                '<td> <button id ="startPopUPAutoProcessingBtn" value = "' + training.TrainingID + '" > Start Automatic Processing </button> </td>' +
                                '</tr>';
                            tableBody.append(row);
                        });
                        $('div#emptyListHeader').html('').hide();
                        $('div#trainingsToBeProcessedTableDiv').show();
                        resolve();
                    }
                }
                else {
                    toastr.error("error in loading training list");
                    reject();
                }
            }).catch((error) => {
                console.error(error);
                toastr.error("System Error");
                reject();
            });
        });
    }

    //function to display popup when buttn is clicked
    $('#trainingsToBeProcessedTable tbody').on("click", "#startPopUPAutoProcessingBtn", function () {
        var row = $(this).closest("tr");
        var selectedTrainingID = $(this).val();
        var selectedTrainingName = row.find("td:eq(0)").text();
        var selectedTrainingNumberOfSeats = row.find("td:eq(3)").text();

        var URL = "/Training/GenerateFinalListOfSelectedEmployees";
        var trainingIDObj = { trainingId: selectedTrainingID };
        var serverCall = new ServerCall({ url: URL, parameters: trainingIDObj, callMethod: "POST" });
        serverCall.fetchApiCall().then((response) => {
            if (response) {
                var listOfAccepted = response.listOfAcceptedEmployees;
                var listOfRejected = response.listOfRejectedEmployees;
                var tableBodyAccepted = $("#tableListOfAccepted tbody");
                var tableBodyRejected = $("#tableListOfRejected tbody");
                tableBodyAccepted.empty();
                tableBodyRejected.empty();
                $('#trainingsToBeProcessedTableDiv').hide();
                if (listOfAccepted.length==0) {
                    //meaning that this training has not received any trainings approved by their managers
                    $('#subpopupHeader').html("<h2>Looks like nobody has applied for " + selectedTrainingName + ".</h2>");
                    $('div#popupListOfAcceptedTableDiv').hide();
                    $('div#popupListOfRejectedTableDiv').hide();
                    $('button#confirmAutomaticProcessing').hide();
                    $('div#popupStartAutoProcessingDiv').show();
                }
                else {
                    //edit header accordingly
                    $('#subpopupHeader').html("<h2>List of Selected and Rejected Employees for " + selectedTrainingName + "</h2> <hr />");
                    $('#subpopupHeader').append("<h4>Training Number of seats : " + selectedTrainingNumberOfSeats + " , Number of applications selected : " + listOfAccepted.length + "</h4>");

                    //populate acceptedTable
                    $.each(listOfAccepted, function (index, application) {
                        var row = '<tr>' +
                            '<td>' + application.FirstName + ' ' + application.LastName + '</td>' +
                            '<td>' + application.Email + '</td>' +
                            '<td>' + formatDateTime(application.DateRegistered) + '</td>' +
                            '</tr>';
                        tableBodyAccepted.append(row);
                    });

                    //populate rejectedTable is not null
                    if (listOfRejected && listOfRejected.length > 0) {
                        $.each(listOfRejected, function (index, application) {
                            var row = '<tr>' +
                                '<td>' + application.FirstName + ' ' + application.LastName + '</td>' +
                                '<td>' + application.Email + '</td>' +
                                '<td>' + formatDateTime(application.DateRegistered) + '</td>' +
                                '</tr>';
                            tableBodyRejected.append(row);

                        });
                        $('#tableListOfRejected').show();
                        $('div#popupListOfRejectedTableDiv').show();
                    } else {
                        $('#EmptyRejectedListHeader').html("No Applied Employees are rejected");
                        $('#tableListOfRejected').hide();
                    }
                    //need to show and hide appropriate divs
                    $('div#popupStartAutoProcessingDiv').show();
                    $('div#popupListOfAcceptedTableDiv').show();
                    $('div#popupListOfRejectedTableDiv').show();
                    $('button#confirmAutomaticProcessing').attr("value", selectedTrainingID).show();
                }
            }
            else {
                toastr.error("error could not load data");
            }
        }).catch((error) => {
            console.error(error);
            toastr.error("error could not get details");
        })
    });

    //function when confirm Automatic processing is pressed
    $(document).on("click", "button#confirmAutomaticProcessing", function () {
        var selectedTrainingID = $(this).val();
        var URL = "/Training/ConfirmAutomaticSelection";
        var trainingIDobj = { trainingID: selectedTrainingID }
        var serverCall = new ServerCall({ url: URL, parameters: trainingIDobj, callMethod: "POST" });
        //show spinner while waiting for backend call
        $('#trainingsToBeProcessed').hide();
        var spinnerContainer = document.getElementById('spinner-container');
        spinnerContainer.innerHTML = '<div class="spinner"></div>';
        serverCall.fetchApiCall().then((response) => {
            spinnerContainer.innerHTML = '';
            if (response == true) {
                Swal.fire({
                    icon: 'success',
                    title: 'Selected Employees have been registered to Training.',
                    text: 'Press Ok to continue',
                    allowOutsideClick: false,
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location = "/Admin/AutomaticProcessingView";
                    }
                });
            } else {
                $('#trainingsToBeProcessed').show();
                toastr.error("Error, could not start automatic processing for this training");
            }
        }).catch((error) => {
            console.log(error);
            spinnerContainer.innerHTML = '';
            $('#trainingsToBeProcessed').show();
            toastr.error("Error, could not start automatic processing for this training");
        })
    });

    //function for close popup button
    $(document).on("click", "button#cancelBtn", function () {
        $('#EmptyRejectedListHeader').html("");
        $('button#confirmAutomaticProcessing').attr("value", "");
        $('div#popupStartAutoProcessingDiv').hide();
        $('div#trainingsToBeProcessedTableDiv').show();
    })

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

    //flow
    populateTableTrainings().then(() => {
        $('div#trainingsToBeProcessed').show();
    })
});