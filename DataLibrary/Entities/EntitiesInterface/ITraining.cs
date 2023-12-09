using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities.EntitiesInterface
{
    public interface ITraining
    {
         int TrainingID { get; set; }
         string TrainingName { get; set; }
         int Capacity { get; set; }
         DateTime? ClosingDate { get; set; }
         string TrainingStatus { get; set; }
         DateTime? TrainingStartDate { get; set; }


    }
}
