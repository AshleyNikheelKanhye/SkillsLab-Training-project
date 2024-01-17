$(document).ready(function () {
    $('nav .nav-container a').removeClass('active');
    $('#adminViewUsers').addClass('active');

    $.ajax({
        url: '/Admin/GetAllUsers',
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            $('div#listOfEmployeesDiv').show();
            $('#allEmployeesTable').DataTable({
                data: data,
                columns: [
                    {
                        data: null,
                        render: function (data, type, row) {
                            return row.FirstName + ' ' + row.LastName;
                        }
                    },
                    { data: 'Email' },
                    { data: 'PhoneNo' },
                    { data: 'DepartmentName' },
                    {
                        data: 'UserID',
                        render: function (data, type, row) {
                            return '<button id="viewEmployeeDetailsBtn" value="' + data + '">More Details</button>';
                        }
                    },
                ],
                ordering: false
            })
        },
        error: function (error) {
            console.log('Error: ', error);
            toastr.error('Error in Loading page');
        }
    });

    $('#allEmployeesTable tbody').on("click", "#viewEmployeeDetailsBtn", function () {
        var URL = "/Admin/GetUserDetails";
        var row = $(this).closest("tr");
        var selectedEmployeeName = row.find("td:eq(0)").text();
        var selectedUserID = $(this).val();
        var userObj = { UserID: selectedUserID };
        var serverCall = new ServerCall({ url: URL, parameters: userObj, callMethod: "POST" });
        serverCall.fetchApiCall().then((response) => {
            if (response) {
                $('#listOfEmployeesDiv').hide();
                $('#h2headerDetails').html(selectedEmployeeName + " Information");

                //user details
                var UserDetailsHtmlBody = "<h5>" +
                    "Name : " + response.FirstName + " " + response.LastName + "<br>" +
                    "Email : " + response.Email + "<br>" +
                    "NIC : " + response.NIC + "<br>" +
                    "Phone Number : " + response.PhoneNo + "<br>" +
                    "Department : " + response.DepartmentName + "<br>" +
                    "</h5>";
                $('#userDetailsDiv').html(UserDetailsHtmlBody);


                //list of roles
                var roleTbody = $("#userRolesTable tbody");
                roleTbody.empty();
                $.each(response.listOfRoles, function (index, role) {
                    var row = '<tr>' +
                        '<td>' + role.RoleName + '</td>' +
                        '</tr>'
                    roleTbody.append(row);
                });


                //list of Qualifications
                var qualificationTbody = $("#userQualificationTable tbody");
                var qualificationHeader = $("#userQualificationHeaderDiv");
                qualificationHeader.empty();
                qualificationTbody.empty();
                if (response.listOfQualifications.length === 0) {
                    qualificationHeader.html("<h3>" + selectedEmployeeName + " has no qualification uploaded</h3>");
                    $('#userQualificationTableDiv').hide();
                }
                else {
                    qualificationHeader.html("<h3>" + selectedEmployeeName + " Qualifications</h3>");
                    $.each(response.listOfQualifications, function (index, qualification) {
                        var row = '<tr>' +
                            '<td>' + qualification.Details + '</td>' +
                            '<td>' + qualification.FileName + '</td>' +
                            '<td> <a href="/Prerequisite/DownloadQualification?userID=' + qualification.UserID + '&prerequisiteID=' + qualification.PrerequisiteID + '" target="_blank">Download File </a></td>' +
                            '</tr>';
                        qualificationTbody.append(row);
                    });
                    qualificationTbody.show();
                    $('#userQualificationTableDiv').show();
                }


                //list of enrollments
                var enrollmentTbody = $("#userEnrollmentTable tbody");
                var enrollmentHeader = $("#userEnrollmentHeaderDiv");
                enrollmentHeader.empty();
                enrollmentTbody.empty();
                if (response.listOfTrainingEnrolled.length === 0) {
                    enrollmentHeader.html("<h3>" + selectedEmployeeName + " has not enrolled in any training</h3>");
                    $('#userEnrollmentTableDiv').hide();
                }
                else {
                    enrollmentHeader.html("<h3>" + selectedEmployeeName + " Enrollments</h3>");
                    $.each(response.listOfTrainingEnrolled, function (index, enrollment) {
                        var row = '<tr>' +
                            '<td>' + enrollment.TrainingName + '</td>' +
                            '<td>' + formatDateTime(enrollment.DateRegistered) + '</td>' +
                            '<td>' + enrollment.ManagerStatus + '</td>' +
                            '<td>' + enrollment.FinalStatus + '</td>' +
                            '</tr>';
                        enrollmentTbody.append(row);
                    });
                    enrollmentTbody.show();
                    $('#userEnrollmentTableDiv').show();
                }
                $('#popupViewUserDiv').show();
            } else {
                toastr.error("Error in loading User Details");
            }
        }).catch((error) => {
            console.error("could not load user details");
            toastr.error('could not load user details');
        });
    });

    $(document).on("click", "button#goBackBtn", function () {
        $("#popupViewUserDiv").hide();
        $("#listOfEmployeesDiv").show();
    })

    function formatDateTime(date) {
        if (date) {
            var parsedDate = new Date(parseInt(date.substr(6)));
            return parsedDate.toLocaleDateString() + ' ' + parsedDate.toLocaleTimeString();
        } else {
            return 'No date';
        }
    }

});