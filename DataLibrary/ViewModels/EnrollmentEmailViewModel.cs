using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.ViewModels
{
    public class EnrollmentEmailViewModel
    {
        public int UserID { get; set; }
        public string Email { get; set; } //user lastname
        public string FirstName { get; set; } //user firstName
        public string LastName { get; set; } 
        public string TrainingName { get; set; }    

    }
}
