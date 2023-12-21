using DataLibrary.Entities.EntitiesInterface;
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
        public DateTime DateRegistered { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string FinalStatus { get; set; }
        public string ManagerStatus { get; set; }
        public bool IsActive { get; set; }
    }
}