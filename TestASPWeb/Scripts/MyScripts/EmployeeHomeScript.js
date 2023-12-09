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




















