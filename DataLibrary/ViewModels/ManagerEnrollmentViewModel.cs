using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.ViewModels
{
    public class ManagerEnrollmentViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string TrainingName {  get; set; }
        public DateTime ClosingDate { get; set; }
        public DateTime TrainingStartDate { get; set; }
        public DateTime DateRegistered { get; set; }
        public int EnrollmentID { get; set; }
        public string ManagerStatus {  get; set; }
        public string FinalStatus {  get; set; }

    }
}
