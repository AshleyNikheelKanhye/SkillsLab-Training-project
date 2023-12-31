using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.ViewModels
{
    public class EmployeeApplicationViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserID { get; set; } 
        public string Email {  get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }  
        public DateTime DateRegistered { get; set; }
        public string ManagerStatus {  get; set; }
        public string FinalStatus { get; set; }

    }
}
