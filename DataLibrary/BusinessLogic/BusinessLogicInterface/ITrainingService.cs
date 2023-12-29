using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repository.RepoInterfaces;
using DataLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic.BusinessLogicInterface
{
    public interface ITrainingService
    {
        IEnumerable<ITraining> GetAll();
        IEnumerable<IPrerequisite> GetPrerequisites(int trainingID);
        IEnumerable<TrainingPrerequisiteDepartmentViewModel> GetAllPrerequisitesAndDepartments();
        IEnumerable<ITraining> GetAllElligible(int UserID);
        Task<bool> Add(AddTrainingViewModel addTrainingViewModel);
    }
}
