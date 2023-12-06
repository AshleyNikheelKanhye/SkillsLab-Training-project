document.addEventListener("DOMContentLoaded", () => {
    LoadUserDetails();
})


function LoadUserDetails() {

    localStorage.clear();
    var serverCall = new ServerCall({ url: "/Employee/GetUserDetails", callMethod: "GET" });
    serverCall.fetchApiCall().then(response => {
        if (response.currentUser) {
            var EmployeeDetails = document.getElementById("userDetails");
            console.log(response);
            var userEmail = response['currentUser']['Email'];
            EmployeeDetails.innerHTML = userEmail;
        }
    });

}