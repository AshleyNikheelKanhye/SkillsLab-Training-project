using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities
{
    public class Enrollment :IEnrollment
    {
        public int EnrollmentID { get; set; }
        public int UserID { get; set; }
        public int TrainingID { get; set; }
        public string TrainingName { get; set; }
        public DateTime ClosingDate { get; set; }  //TrainingClosingDate
        public DateTime TrainingStartDate { get; set; } //Training Start Date
        public int Capacity { get; set; } //Training Capacity
        public string DepartmentName { get; set; } //favoured DepartmentName
        public DateTime DateRegistered { get; set; }
        public DateTime ?ApprovalDate { get; set; }
        public string FinalStatus { get; set; }
        public string ManagerStatus { get; set; }
        public bool ?IsActive { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public bool IsAutomaticProcessed { get; set; }
    }
}