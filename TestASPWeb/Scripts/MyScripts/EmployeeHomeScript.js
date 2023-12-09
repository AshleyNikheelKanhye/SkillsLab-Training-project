$(document).ready(function () {
    // Function to load the partial view content

    function makeAJAXCall(viewURL) {
        
        return new Promise((resolve, reject) => {

            $.ajax({
                url: viewURL, // Adjust the URL based on your route
                type: 'GET',
                success: function (data) {
                    resolve(data);
                },
                error: function (error) {
                    reject(error);
                    alert('Error loading content.');
                }
            })
        });

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



    function populateTable() {
        var URL = "/Training/getAll";

        getAJAXCall(URL).then((response) => {
            if (response) {
                console.log(response);
                //var jsonString = JSON.stringify(response);
                //alert(jsonString);
                //toastr.success("could load table");
                var tableBody = $("#trainingTable tbody");
                tableBody.empty();

                $.each(response, function (index, training) {
                    var row = '<tr>' +
                        '<td>' + training.TrainingID + '</td>' +
                        '<td>' + training.TrainingName + '</td>' +
                        '<td>' + training.Capacity + '</td>' +
                        '<td>' + formatDateTime(training.ClosingDate) + '</td>' +
                        '<td>' + training.TrainingStatus + '</td>' +
                        '<td>' + formatDateTime(training.TrainingStartDate) + '</td>' +
                        '</tr>';

                    tableBody.append(row);
                });
            }
        }).catch((error) => {
            console.error(error);
            toastr.error("error cannot load data");
        });
    }

    function formatDateTime(date) {
        if (date) {
            var parsedDate = new Date(parseInt(date.substr(6)));
            return parsedDate.toLocaleDateString() + ' ' + parsedDate.toLocaleTimeString();
        } else {
            return '';
        }
    }


    async function LoadViews(viewURL) {
        try {
            const result = await makeAJAXCall(viewURL);
            $('#mainContent').html(result);
            
        } catch (error) {
            // Handle errors here
            console.error(error);
        }
    }





    function removeAllActiveClasses() {
        const navLinks = document.querySelectorAll('.navbar a');
        navLinks.forEach(link => link.classList.remove('active'));
    }
    function logout() {
        // Add your logout logic here
        alert('Logout button clicked!');
    }

    // Add click event handlers to your navigation links
    $('#homeLink').click(function (e) {
        e.preventDefault();
        removeAllActiveClasses();
        $(this).addClass("active");
        var url = '/Employee/GetHomeView';
        LoadViews(url);
    });

    $('#profileLink').click(function (e) {
        e.preventDefault();
        removeAllActiveClasses();
        $(this).addClass("active");
        var url = '/Employee/GetProfileView';
        LoadViews(url);
    });

    $('#viewTrainingLink').click(function (e) {
        e.preventDefault();
        removeAllActiveClasses();
        $(this).addClass("active");
        var url = '/Employee/GetTrainingView';
        LoadViews(url);
        populateTable();
  
        
    });

    $('#logoutLink').click(function (e) {
        e.preventDefault();
        removeAllActiveClasses();
        $(this).addClass("active");
        logout();
    });




    //since button has been loaded later
/*    $(document).on("click", "button#testbtn", function (e) {
        e.preventDefault();
        alert('test btn has been clicked');
    });
*/


    //flow of javascript from start
    removeAllActiveClasses();
    var url = '/Employee/GetHomeView';
    LoadViews(url);
    $('#homeLink').addClass("active");
});




















