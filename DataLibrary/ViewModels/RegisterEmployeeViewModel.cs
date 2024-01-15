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
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]\d{12}[A-Z]$")]
        public string NIC { get; set; }

        [Required]
        [RegularExpression(@"^5\d{7}$")]
        public int PhoneNo { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int DepartmentID { get; set; }

        [Required]
        public int ManagerID { get; set; }
        public string Role { get; set; }

    }
}
