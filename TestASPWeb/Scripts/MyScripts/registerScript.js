document.addEventListener("DOMContentLoaded", function () {
    console.log('i am loaded');
    var serverCall = new ServerCall({
        url: "/User/GetDepartments",
        callMethod: "GET" 
    });

    serverCall.fetchApiCall()
        .then(response => {

            alert(JSON.stringify(response));
            // Populate the department dropdown with fetched data
            

            const departmentDropdown = document.getElementById('department');

            response.listDepartments.forEach(department => {
                const option = document.createElement('option');
                option.value = department.DepartmentID;
                option.textContent = department.DepartmentName;
                departmentDropdown.appendChild(option);
            });
        })
        .catch(error => console.error('Error fetching departments:', error));
});




