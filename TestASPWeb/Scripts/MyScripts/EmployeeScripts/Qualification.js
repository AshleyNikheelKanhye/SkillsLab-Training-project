$(document).ready(function () {
    $('nav .nav-container a').removeClass('active');
    $('#Qualifications').addClass('active');

    function getQualifications() {
        var URL = "/Employee/GetEmployeeQualifications"; 
        
        
        var table = $("#myQualificationTable tbody");
        table.empty();
        var serverCall = new ServerCall({ url: URL, callMethod: "POST" });
        serverCall.fetchApiCall().then((response) => {
            var results = response.result;
            if (results && results.length > 0) {

                $.each(results, function (index, qualification) {
                    var row = '<tr>' +
                        '<td>' + qualification.Details + '</td>' +
                        '<td>' + qualification.FileName + '</td>' +
                        '<td> <a href="/Employee/DownloadQualification?prerequisiteID=' + qualification.PrerequisiteID + '" target="_blank">Download File </a></td>' +
                        '<td> <button id="updateQualification" value="' + qualification.PrerequisiteID + '">Update</button> </td>' +
                        + '</tr>';
                    table.append(row);
                });
                $('div#viewQualifications').show();
            }
            else {
                $('div#noQualifications').show();
            }
        });
    }

    function loadAddQualifications() {
        var URL = "/Prerequisite/GetPrerequisitesNotInEmployee";
        var serverCall = new ServerCall({ url: URL, callMethod: "GET" });
        const qualificationsDropdown = document.getElementById('listOfQualifications');
        serverCall.fetchApiCall().then(response => {
            if (response.listQualifications) { 
                response.listQualifications.forEach(qualification => {
                    const option = document.createElement('option');
                    option.value = qualification.PrerequisiteID;
                    option.textContent = qualification.Details;
                    qualificationsDropdown.appendChild(option);
                });
                $('div#AddQualification').show();
            }
            if (response.error) {
                toastr.error("could not load list of Qualifications");
            }
        })
            .catch(error => {
                console.error("error fetching qualifications ", error);
                toastr.error('could not load list of qualifications');
            });
    }

    $('button#btnAddQualification').click(function () {
        $('.content').hide();
        loadAddQualifications();
    });


    $('button#uploadbtn').click(function () {
        var fileInput = document.getElementById('fileInput');
        var file = fileInput.files[0];
        var prerequisiteID = document.getElementById("listOfQualifications").value;
        if (prerequisiteID == "") { toastr.error("Please select a qualification"); return; }

        if (file) {
            const formData = new FormData();
            formData.append('file', file);
            formData.append('prerequisiteID', prerequisiteID);

            $.ajax({
                url: '/Employee/UploadQualifications',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.result == true) {
                        Swal.fire({
                            icon: 'success',
                            title: 'File Uploaded',
                            text: 'Press OK to continue',
                            allowOutsideClick: false,
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location = "/Employee/QualificationsView";
                            }
                        });
                    } else {
                        toastr.error("Could not upload pdf");
                    }
                },
                error: function (error) {
                    console.log(error);
                    toastr.error("Error on uploading file");
                }
            });
        }
        else {
            toastr.error("please upload your pdf file");
        }
    });



    $('#myQualificationTable tbody').on("click", "#updateQualification", function () {
        $('.content').hide();
        var row = $(this).closest("tr");
        var selectedQualificationId = $(this).val();
        var selectedQualificationName = row.find("td:eq(0)").text();
        $('h2#updateQualificationName').html("<h2>Update your " + selectedQualificationName + " PDF</h2>");
        $('button#uploadbtnForUpdate').attr("value", selectedQualificationId);
        $("#UpdateQualificationDIV").show();
    });

    $('button#uploadbtnForUpdate').click(function () {
        var fileInput = document.getElementById('fileInputUpdate');
        var file = fileInput.files[0];
        var prerequisiteID = $(this).val();
        var URL = "/Employee/UpdateQualification";
        if (file) {
            const formData = new FormData();
            formData.append('file', file);
            formData.append('prerequisiteID', prerequisiteID);

            $.ajax({
                url: URL,
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.result == true) {
                        Swal.fire({
                            icon: 'success',
                            title: 'File Update',
                            text: 'Press OK to continue',
                            allowOutsideClick: false,
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location = "/Employee/QualificationsView";
                            }
                        });
                    } else {
                        toastr.error("Could not upload pdf");
                    }
                },
                error: function (error) {
                    console.log(error);
                    toastr.error("error on updating file");
                }
            })
        } else {
            toastr.error("please select a pdf file");
        }
    });


    getQualifications();
});