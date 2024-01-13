$(document).ready(function () {
    $('nav .nav-container a').removeClass('active');
    $('#Qualifications').addClass('active');

    function getQualifications() {
        var URL = "/Employee/GetEmployeeQualifications"; //TODO : move that to Prerequisite Controller
        
        
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
                        '<td> <a href="/Employee/DownloadQualification?prerequisiteID=' + qualification.PrerequisiteID + '" target="_blank">Download File </a></td>'
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
            if (response.listQualifications) { // TODO : cater for when that user has all the qualifications , display accordingly
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
            console.log('uploading file please wait');

            $.ajax({
                url: '/Employee/UploadQualifications',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
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
                },
                error: function (error) {
                    console.log(error);
                    alert('an error occured check console');
                }
            });
        }
        else {
            toastr.error("please upload your pdf file");
        }
    });

    getQualifications();
});