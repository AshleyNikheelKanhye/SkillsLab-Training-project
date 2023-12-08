$(document).ready(function () {
    // Function to load the partial view content


    function loadPartialViewContent() {
        $.ajax({
            url: '/Employee/GetHomeView', // Adjust the URL based on your route
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
        loadPartialViewContent();
    });


    // Add similar event handlers for other navigation links
    // $('#profileLink').click(function (e) { ... });
    // $('#trainingsLink').click(function (e) { ... });



    //flow of javascript from start
    removeAllActiveClasses();
    loadPartialViewContent();
    $('#homeLink').addClass("active");
});

























/*document.addEventListener("DOMContentLoaded", () => {
    LoadUserDetails();
})

function LoadUserDetails() {
    var serverCall = new ServerCall({ url: "/Employee/GetUserDetails", callMethod: "GET" });
    serverCall.fetchApiCall().then(response => {
        if (response.currentUser) {
            //var EmployeeDetails = document.getElementById("userDetails");
            console.log(response);
            //var userEmail = response['currentUser']['Email'];
            //document.getElementById('FirstName').innerHTML = response['currentUser']['FirstName'];
            //document.getElementById('LastName').innerHTML = response['currentUser']['LastName'];
            
        }
    });
}*/