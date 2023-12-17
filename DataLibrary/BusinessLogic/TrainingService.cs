using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repository.RepoInterfaces;
using DataLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public class TrainingService : ITrainingService
    {
        ITrainingDAL _trainingRepo;
        IPrerequisiteDAL _prerequisiteDAL;
        IDepartmentDAL _departmentDAL;
        public TrainingService(ITrainingDAL trainingRepo,IPrerequisiteDAL prerequisiteRepo,IDepartmentDAL departmentRepo)
        {
            this._trainingRepo = trainingRepo;
            this._prerequisiteDAL = prerequisiteRepo;
            this._departmentDAL = departmentRepo;
            
        }
        public IEnumerable<ITraining> GetAll()
        {
            var listOfAllTraining= _trainingRepo.GetAll();
            return listOfAllTraining;
        }

        public IEnumerable<IPrerequisite> GetPrerequisites(int trainingID)  //do not need that , delete this method, to preserve single responsiblity
        {
            try
            {
                return _prerequisiteDAL.GetPrerequisites(trainingID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<TrainingPrerequisiteDepartmentViewModel> GetAllPrerequisitesAndDepartments()
        {
            List<TrainingPrerequisiteDepartmentViewModel> list = new List<TrainingPrerequisiteDepartmentViewModel>();

            var listOfAllTraining = _trainingRepo.GetAll();  //returns a list of ITraining

            foreach (var training in listOfAllTraining)
            {
                var listOfPrerequisites = _prerequisiteDAL.GetPrerequisites(training.TrainingID).ToList();
                var listOfDepartments = _departmentDAL.GetDepartmentsForTraining(training.TrainingID).ToList();

                list.Add(new TrainingPrerequisiteDepartmentViewModel() { training = training, listOfPrerequisites = listOfPrerequisites, listOfDepartments = listOfDepartments });
            }

            return list;
        }


    }
}
