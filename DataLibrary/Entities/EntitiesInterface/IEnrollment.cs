using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities.EntitiesInterface
{
    public interface IEnrollment
    {
        int EnrollmentID { get; set; }  
        int UserID { get; set; }    
        int TrainingID { get; set; }
        DateTime DateRegistered { get; set; }
        DateTime ApprovalDate {  get; set; }
        string FinalStatus { get; set; }
        string ManagerStatus { get; set; }
        bool IsActive { get; set; } 

    }
}
