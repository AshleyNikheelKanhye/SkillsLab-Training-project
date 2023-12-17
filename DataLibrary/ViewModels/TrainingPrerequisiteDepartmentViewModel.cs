using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.ViewModels
{
    public class TrainingPrerequisiteDepartmentViewModel
    {        
        public ITraining training {get; set;}
        
        public List<IPrerequisite> listOfPrerequisites { get; set;}

        public List<IDepartment> listOfDepartments { get; set;}
    }
}
