using DataLibrary.Entities.EntitiesInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.ViewModels
{
    public class RegisterEmployeeViewModel 
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "NIC is required")]
        public string NIC { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^5\d{7}$", ErrorMessage = "Invalid phone number format")]
        public int PhoneNo { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Manager is required")]
        public int ManagerID { get; set; }
        public string Role { get; set; }

    }
}
