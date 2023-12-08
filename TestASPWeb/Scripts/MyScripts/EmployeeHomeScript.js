$(document).ready(function () {
    // Function to load the partial view content
    function LoadViews(viewURL) {
        $.ajax({
            url: viewURL, // Adjust the URL based on your route
            type: 'GET',
            success: function (data) {
                // Update the mainContent div with the received HTML content
                $('#mainContent').html(data);
            },
            error: function () {
                alert('Error loading content.');
            }
        });
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
    });

    $('#logoutLink').click(function (e) {
        e.preventDefault();
        removeAllActiveClasses();
        $(this).addClass("active");
        logout();
    });


    //flow of javascript from start
    removeAllActiveClasses();
    var url = '/Employee/GetHomeView';
    LoadViews(url);
    $('#homeLink').addClass("active");
});




















