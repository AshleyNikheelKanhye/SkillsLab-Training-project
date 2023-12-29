using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.ViewModels
{
    public class AddTrainingViewModel
    {
        public string TrainingName { get; set; }    
        public string TrainingCapacity { get; set; }
        public DateTime DeadlineRegistration {  get; set; }
        public DateTime StartingDate { get; set; }
        public int Department {  get; set; } //department id
        public List<int> PrerequisiteList { get; set; }
    }
}
