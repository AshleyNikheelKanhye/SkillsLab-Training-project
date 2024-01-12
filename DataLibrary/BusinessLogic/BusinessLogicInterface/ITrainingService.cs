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
        Task<IEnumerable<ITraining>> GetUnprocessedTrainings();
        Task<AutomaticProcessingViewModel> GenerateFinalListOfSelectedEmployees(int trainingID);
        Task<bool> ConfirmAutomaticSelection(int trainingID);
        Task<IEnumerable<ITraining>> getUpcomings();
        Task<ITraining> GetTrainingToUpdateDetails(int trainingID);
        Task<bool> Update(UpdateTrainingViewModel updateTrainingViewModel);
        Task<bool> Delete(int trainingID);
        Task<IEnumerable<ITraining>> GetCompletedTrainings();
        Task<IEnumerable<ITraining>> GetDeletedTrainings();
        Task<IEnumerable<SelectedEmployeeViewModel>> GetSelectedEmployees(int trainingID);
        Task<string> GetTrainingDescription(int trainingID);
        Task QuartzAutomaticProcessing();


    }
}
