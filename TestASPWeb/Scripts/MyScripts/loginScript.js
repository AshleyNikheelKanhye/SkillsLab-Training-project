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

    //validate Email
    if (!isValidEmail(email)) {
        toastr.error("Invalid email address");
        return;
    }
    // Validate password
    if (!isValidPassword(password)) {
        toastr.error("Please Enter Password");
        return;
    }
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
        } else if (response.pending) {
            window.location = response.url;
        }
        else {
            toastr.error("Invalid Credentials");
        }
    })
}

function isValidEmail(email) {
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return email.trim() !== "" && emailRegex.test(email);
}
function isValidPassword(password) {
    return password.trim() !== "";
}
function createAccount() {
    window.location.href = "/User/Register/";
}
