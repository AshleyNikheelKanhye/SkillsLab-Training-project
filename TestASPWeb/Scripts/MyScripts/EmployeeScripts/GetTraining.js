$(document).ready(function () {
    $('nav .nav-container a').removeClass('active');
    $('#Training').addClass('active');



    $('#myTrainings').change(function () {
        $('.content').hide();
        $('div#myTrainingsContent').show();
    });


    $('#trainingsAvailable').change(function () {
        $('.content').hide();
        populateTableMyTrainings().then(() => {
            $('div#trainingsAvailableContent').show();
        });
    });


    $('#allTrainings').change(function () {
        $('.content').hide();
        populateTable().then(() => {
            $('div#allTrainingsContent').show();
        });
    });

    //dropdown for MyTrainings
    $('#mySelect').change(function () {
        var selectedOption = $(this).val();
        $('div#EmptyListHeaderdiv').empty().hide();
        $('div#selectContentDiv').hide();
        var URL = "";
        if (selectedOption == "1") {
            URL = "/Enrollment/GetFinalApprovedTrainings";
            $('div#headerSelectDiv').html('<h2>Registered Trainings </h2>');
        }
        if (selectedOption == "2") {
            URL = "/Enrollment/GetManagerApprovedTrainings";
            $('div#headerSelectDiv').html('<h2>Trainings Approved by Manager</h2>');
        }
        if (selectedOption == "3") {
            URL = "/Enrollment/GetPendingTrainings";
            $('div#headerSelectDiv').html('<h2>Trainings pending to be Approved by Manager</h2>');
        }
        if (selectedOption == "4") {
            URL = "/Enrollment/GetDeclinedTrainings";
            $('div#headerSelectDiv').html('<h2>Trainings Declined by Manager or Automatic selection</h2>');
        }
        populateMyTrainingDiv(URL).then(() => {
            $('div#selectContentDiv').show();
        });

    });



    function formatDateTime(date) {
        if (date) {
            var parsedDate = new Date(parseInt(date.substr(6)));
            return parsedDate.toLocaleDateString() + ' ' + parsedDate.toLocaleTimeString();
        } else {
            return 'No date';
        }
    }

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

    //function to populate Table Trainings ellible to apply (this function name is not correct change the function name)
    function populateTableMyTrainings() {
        return new Promise((resolve, reject) => {
            var URL = "/Training/GetAllElligilbe";
            getAJAXCall(URL).then((response) => {
                if (response) {
                    if (response.length === 0) {
                        $('div#NoAvailableTrainingsForYou').html("<h2>Looks like there are no Trainings available for you based on your Qualifications at the moment<h2>");
                        $('div#tableTrainingsAvailable').hide();
                        $('div#NoAvailableTrainingsForYou').show();
                        resolve();
                    } else {
                        var tableBody = $("#availableTraining tbody");
                        tableBody.empty();
                        $.each(response, function (index, training) {
                            var row = '<tr>' +
                                '<td>' + training.TrainingName + '</td>' +
                                '<td>' + training.Capacity + '</td>' +
                                '<td>' + formatDateTime(training.ClosingDate) + '</td>' +
                                '<td>' + formatDateTime(training.TrainingStartDate) + '</td>' +
                                '<td>' + training.Duration + '</td>' +
                                '<td data-DepartmentID = "' + training.DepartmentID + '">' + training.DepartmentName + '</td>' +
                                '<td> <button id ="Applybtn" value = "' + training.TrainingID + '" > Apply </button> </td>' +
                                '<td hidden>' + training.Description + '</td>' +
                                '</tr>';
                            tableBody.append(row);
                        });
                        $('div#NoAvailableTrainingsForYou').html("");
                        $('div#NoAvailableTrainingsForYou').hide();
                        $('div#tableTrainingsAvailable').show();
                        resolve();
                    }
                }
            }).catch((error) => {
                console.error(error);
                toastr.error("error cannot load data");
                reject();
            });
        });
    }


    //function to populate MyTrainings
    function populateMyTrainingDiv(URL) {
        return new Promise((resolve, reject) => {
            getAJAXCall(URL).then((response) => {
                if (response) {
                    if (response.length === 0) {
                        $("div#EmptyListHeaderdiv").html('<h4>No records were found</h4>');
                        $('#myTrainingTableDiv').hide();
                        $("div#EmptyListHeaderdiv").show();
                        resolve();
                    }
                    else {
                        $('#EmptyListHeaderdiv').html("").hide();
                        if ($.fn.DataTable.isDataTable('#myTrainingTable')) {
                            $('#myTrainingTable').DataTable().clear().destroy();
                        }

                        $('#myTrainingTable').DataTable({
                            data: response,
                            columns: [
                                { data: 'TrainingName' },
                                {
                                    data: 'TrainingStartDate',
                                    render: function (data, type, row) {
                                        return formatDateTime(data);
                                    }
                                },
                                { data: 'Duration' },
                                {
                                    data: 'ManagerStatus',
                                    render: function (data, type, row) {
                                        if (data == 'Approved') { return '<strong style="color:green;">' + data + '</strong>'; }
                                        if (data == 'Disapproved') { return '<strong style="color:red;">' + data + '</strong>'; }
                                        else { return '<strong style="color:orange;">' + data + '</strong>'; }
                                    }
                                },
                                {
                                    data: 'FinalStatus',
                                    render: function (data, type, row) {
                                        if (data == 'Approved') { return '<strong style="color:green;">' + data + '</strong>'; }
                                        if (data == 'Disapproved') { return '<strong style="color:red;">' + data + '</strong>'; }
                                        else { return '<strong style="color:orange;">' + data + '</strong>'; }
                                    }
                                },
                            ],
                            ordering: false,
                            autoWidth: true
                        });
                        $('#myTrainingTableDiv').show();
                        resolve();
                    }
                }
                else {
                    toastr.error("backend error");
                    reject();
                }
            }).catch((error) => {
                console.error(error);
                toastr.error("error cannot load data");
                reject();
            });
        });
    }

    //function to populate Table All trainings
    function populateTable() {
        return new Promise((resolve, reject) => {
            var URL = "/Training/getAll";
            getAJAXCall(URL).then((response) => {
                if (response) {
                    if ($.fn.DataTable.isDataTable('#trainingTable')) {
                        // DataTable is already initialized, destroy it
                        $('#trainingTable').DataTable().clear().destroy();
                    }
                    $('#trainingTable').DataTable({
                        data: response,
                        columns: [
                            { data: 'TrainingName' },
                            { data: 'Capacity' },
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
                            { data: 'Duration' },
                            { data: 'DepartmentName' },
                            {
                                data: 'TrainingID',
                                render: function (data, type, row) {
                                    return `<button id="viewPrerequisites" value= ${data}>Details</button>`;
                                }
                            },
                        ],
                        ordering: false,
                        autoWidth: true
                    });
                    resolve();
                }
            }).catch((error) => {
                console.error(error);
                toastr.error("error cannot load data");
                reject();
            });
        });
    }

    //function to be triggered when clicking on view prerequisites button
    $("#trainingTable tbody").on("click", "#viewPrerequisites", function () {
        var URL = "/Training/GetPrerequisites";
        var row = $(this).closest("tr");
        var clickedTrainingName = row.find("td:eq(0)").text();
        var trainingID = $(this).val();
        var trainingobj = { trainingID: trainingID }
        var descriptionURL = "/Training/GetTrainingDescription";
        var serverCall = new ServerCall({ url: URL, parameters: trainingobj, callMethod: "POST" });
        serverCall.fetchApiCall().then((response) => {
            if (response) {
                //populate popup
                response.result.forEach(function (p) {
                    var paragraph = document.createElement('h6');
                    paragraph.textContent = ' -   ' + p.Details;
                    $('div#popupText').append(paragraph);
                });

                $('div#trainingName').html("<h2>Training prerequisites for : " + clickedTrainingName + "</h2>");
                $('div#divAllTrainingTable').fadeOut();
                $('div#popup').fadeIn(1000);
            }
            else {
                toastr.error("error");
            }
        }).catch((error) => {
            console.error(error);
            toastr.error("error");
        });

        var serverCallDescription = new ServerCall({ url: descriptionURL, parameters: trainingobj, callMethod: "POST" });
        serverCallDescription.fetchApiCall().then((response) => {
            if (response) {
                console.log(response);
                $('div#trainingDescription').html(response);
            } else {
                $('div#trainingDescription').html("could not load training description");
            }
        });
    });

    //when applied button is pressed
    $(document).on("click", "button#finalApplyBtn", function () {
        var selectedTrainingID = $(this).val();
        var URL = "/Enrollment/AddEnrollment";
        var trainingObj = { trainingID: selectedTrainingID };
        var serverCall = new ServerCall({ url: URL, parameters: trainingObj, callMethod: "POST" });

        // Show spinner while waiting for the backend call
        $('#applyPopUp').hide();
        var spinnerContainer = document.getElementById('spinner-container');
        spinnerContainer.innerHTML = '<div class="spinner"></div>';

        serverCall.fetchApiCall().then((response) => {
            spinnerContainer.innerHTML = '';
            if (response.result == true) {
                Swal.fire({
                    icon: 'success',
                    title: 'Training Successfully Applied. Email will be sent to inform your Manager ',
                    text: 'Press OK to continue',
                    allowOutsideClick: false,
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location = "/Employee/GetTrainingView";
                    }
                });
            }
            else {
                toastr.error("Error, could not apply for training");
                $('#applyPopUp').show();
            }
        })
    });

    //function to be triggered when clicking on apply for training button
    $("#availableTraining tbody").on("click", "#Applybtn", function () {
        var row = $(this).closest("tr");
        var selectedTrainingID = $(this).val();
        var selectedTrainingName = row.find("td:eq(0)").text();
        var selectedTrainingSeatsAvailable = row.find("td:eq(1)").text();
        var selectedTrainingClosingDate = row.find("td:eq(2)").text();
        var selectedTrainingStartDate = row.find("td:eq(3)").text();
        var selectedTrainingDuration = row.find("td:eq(4)").text();
        var selectedTrainingDepartmentFavoured = row.find("td:eq(5)").text();
        var selectedTrainingDescription = row.find("td:eq(7)").text();


        //modify the popupdiv
        $('#trainingTitleHeader').text("Apply for Training : " + selectedTrainingName);
        var bodyText = "<h4>You have all the qualifications to apply for this training</h4>" +
            "<h6>Once you apply, You will need your Manager's Approval before you can be selected for the Training.</h6>" +
            "<h6>Your Manager will need to approve your request before the Training Enrollment Deadline: <strong>" + selectedTrainingClosingDate + "</strong></h6>" +
            "<h6>This Training favours <strong>" + selectedTrainingDepartmentFavoured + "</strong> Employees</h6>" +
            "<hr />" +
            "<h5>Training Starting Date : <strong>" + selectedTrainingStartDate + "</strong></h5>" +
            "<h5>Number of Seats available : <strong>" + selectedTrainingSeatsAvailable + "</strong></h5>" +
            "<h5>Training Duration (days) :<strong>" + selectedTrainingDuration + "</strong></h5>" +
            "<div>Training Description : <br/> " + selectedTrainingDescription + "</div>" +
            "<hr />" +
            "<h3>You will get Emails whenever there are changes on your application</h3>";
        $('div#applyBodyDiv').html(bodyText);
        $('button#finalApplyBtn').attr("value", selectedTrainingID);
        $('div#tableTrainingsAvailable').hide();
        $('div#applyPopUp').fadeIn(500);
    });


    //close popup view prerequisites
    $(document).on("click", "button#closebtn", function () {
        $('div#trainingName').text('');
        $('div#popupText').text('');
        $('div#popup').fadeOut();
        $('div#divAllTrainingTable').fadeIn();
    });

    //cancel button when applypopup
    $(document).on("click", "button#cancelApplyBtn", function () {
        $('#trainingTitleHeader').text('');
        $('div#applyBodyDiv').html("");
        $('button#finalApplyBtn').attr("value", "");
        $('div#applyPopUp').fadeOut();
        $('div#tableTrainingsAvailable').fadeIn(500);
    });

    //I was testing to get all training with their prerequisites delete that dont need that anymore
    function testFunction() {
        var URL = "/Training/GetAllTrainingWithPrerequisitesAndDepartments";
        getAJAXCall(URL).then((response) => {
            console.log(response);
            alert(JSON.stringify(response));
        });
    }


    //modal
    $(document).on("click", "#registeredInfobtn", function () {
        document.getElementById('id01').style.display = 'block';
    });

    //flow
    $('#mySelect').trigger('change');

});