using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.ViewModels
{
    public class SelectedEmployeeViewModel
    {
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string NIC { get; set; }
        public int PhoneNo { get; set; }
        public string ManagerFirstName {  get; set; }
        public string ManagerLastName { get; set; }
        public string ManagerEmail { get; set; }    

    }
}
