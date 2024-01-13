$(document).ready(function () {
    $('nav .nav-container a').removeClass('active');
    $('#managerHome').addClass('active');

    //populate Table my employees
    $.ajax({
        url: '/Manager/GetEmployeesUnderManager',
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#employeeTable').DataTable({
                data: data,
                columns: [
                    {
                        data: null,
                        render: function (data, type, row) {
                            return row.FirstName + ' ' + row.LastName;
                        }
                    },
                    { data: 'Email' },
                    { data: 'PhoneNo' },
                ],
                ordering: false
            });
        }, error: function (error) {
            console.log(error);
        }
    });
});