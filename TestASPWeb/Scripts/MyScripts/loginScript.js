document.addEventListener("DOMContentLoaded", () => {
    let form = document.querySelector('form');
    form.addEventListener('submit', (e) => {
        e.preventDefault();
        return false;
    });
})

function signIn() {
    var email = document.getElementById("email").value;
    var password = document.getElementById("password").value;

    var authObj = { Email: email, Password: password };
    var serverCall = new ServerCall({ url: "/User/Authenticate", parameters: authObj, callMethod: "POST" });

    
    serverCall.fetchApiCall().then((response) => {
        if (response.result) {
            toastr.success("Authentication Successful");
            window.location = response.url;
        } else if (response.errors) {
            for (var error in response.errors) {
                toastr.error(response.errors[error][0]);
            }
        } else {
            toastr.error("Invalid Credentials");
        }
    })
}



function createAccount() {
    window.location.href = "/User/Register/";
}



