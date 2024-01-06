using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.ViewModels
{
    public class UpdateTrainingViewModel
    {
        public int TrainingId { get; set; }  
        public string TrainingName { get; set; }
        public int TrainingCapacity { get; set; }
        public DateTime DeadlineRegistration {  get; set; }
        public DateTime StartingDate { get; set; }
        public int TrainingDuration { get; set; }
        public string TrainingDescription {  get; set; }




    }
}
