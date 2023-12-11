$(document).ready(function () {
    function makeAJAXCall(viewURL) {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: viewURL,
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

    async function populateTable() {
        var URL = "/Training/getAll";
        getAJAXCall(URL).then((response) => {
            if (response) {
                console.log(response);
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

    async function LoadViews(viewURL) {
        makeAJAXCall(viewURL).then((response) => {
            if (response) {
                console.log("loaded view");
                $('#mainContent').html(response);
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
            return 'No date';
        }
    }
    function removeAllActiveClasses() {
        const navLinks = document.querySelectorAll('.navbar a');
        navLinks.forEach(link => link.classList.remove('active'));
    }

    function logout() {
        window.location.href = "/User/Logout";
    }


    //  click event handlers to navigation links
    $('#homeLink').click(async function (e) {
        e.preventDefault();
        $('#mainContent').css("display", "none");
        removeAllActiveClasses();
        $(this).addClass("active");
        var url = '/Employee/GetHomeView';
        await LoadViews(url);
        $('#mainContent').fadeIn(1000);
    });

    $('#profileLink').click(async function (e) {
        e.preventDefault();
        removeAllActiveClasses();
        $('#mainContent').css("display", "none");
        $(this).addClass("active");
        var url = '/Employee/GetProfileView';
        await LoadViews(url);
        $('#mainContent').fadeIn(1000);
    });

    $('#viewTrainingLink').click( async function (e) {
        e.preventDefault();
        removeAllActiveClasses();
        $('#mainContent').css("display", "none");
        $(this).addClass("active");
        var url = '/Employee/GetTrainingView';

        const x = await LoadViews(url);
        const y = await populateTable();
        Promise.all([x, y]).then(() => {
            $('#mainContent').fadeIn(1000);
        });

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
















