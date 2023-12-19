using DataLibrary.Entities.EntitiesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities
{
    public class Training : ITraining
    {
        public int TrainingID { get; set; }
        public string TrainingName { get; set; }
        public int Capacity { get; set; }
        public DateTime? ClosingDate { get; set; }
        public string TrainingStatus { get; set; }
        public DateTime? TrainingStartDate { get; set; }
        public bool IsActive { get; set; }
    }
}
