using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.ViewModels
{
    public class AddTrainingViewModel
    {
        [Required]
        public string TrainingName { get; set; }

        [Required]
        public string TrainingCapacity { get; set; }

        [Required]
        public DateTime DeadlineRegistration {  get; set; }

        [Required]
        public DateTime StartingDate { get; set; }

        [Required]
        public int Department {  get; set; } //department id

        [Required]
        public List<int> PrerequisiteList { get; set; }

        [Required]
        public int TrainingDuration { get; set; }

        [Required]
        public string TrainingDescription { get; set; }
    }
}
