document.addEventListener("DOMContentLoaded", () => {
    LoadUserDetails();
})


function LoadUserDetails() {

    localStorage.clear();
    var serverCall = new ServerCall({ url: "/Employee/GetUserDetails", callMethod: "GET" });
    serverCall.fetchApiCall().then(response => {
        if (response.employee) {
            var EmployeeDetails = document.getElementById("userDetails");
            console.log(result);
            var userEmail = response['employee']['Email'];
            EmployeeDetails.innerHTML = userEmail;
        }
    });








}