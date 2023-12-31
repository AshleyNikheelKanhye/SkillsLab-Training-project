using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.BusinessLogic.Logger;
using DataLibrary.Entities;
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
        ILogger _logger;
        public TrainingService(ITrainingDAL trainingRepo,IPrerequisiteDAL prerequisiteRepo,IDepartmentDAL departmentRepo,ILogger logger)
        {
            this._trainingRepo = trainingRepo;
            this._prerequisiteDAL = prerequisiteRepo;
            this._departmentDAL = departmentRepo;
            this._logger = logger;
        }
        public IEnumerable<ITraining> GetAll()
        {
            var listOfAllTraining= _trainingRepo.GetAll();
            return listOfAllTraining;
        }

        public IEnumerable<ITraining> GetAllElligible(int UserID)
        {
            var listOfElligibleTrainings = _trainingRepo.GetAllElligible(UserID);
            return listOfElligibleTrainings;
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

        public async Task<bool> Add(AddTrainingViewModel addTrainingViewModel)
        {
            try
            {
                return await _trainingRepo.Add(addTrainingViewModel);                
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex);
                return false;
            }
        }

        public async Task<IEnumerable<ITraining>> GetUnprocessedTrainings()
        {
            try
            {
                return await _trainingRepo.GetUnprocessedTrainings();
            }
            catch(Exception ex) 
            { 
                this._logger.LogError(ex);
                return null;
            }
        }

        public async Task<AutomaticProcessingViewModel> GenerateFinalListOfSelectedEmployees(int trainingID)
        {
            try
            {
                List<EmployeeApplicationViewModel> acceptedList = new List<EmployeeApplicationViewModel>();
                List<EmployeeApplicationViewModel> rejectedList = new List<EmployeeApplicationViewModel>(); 
                AutomaticProcessingViewModel automaticProcessing = new AutomaticProcessingViewModel();
                //get the required training details
                ITraining selectedTraining = await _trainingRepo.GetTraining(trainingID);

                //get all the employees application with manager's approval for trainingID
                List<EmployeeApplicationViewModel> applicationList = await _trainingRepo.GetListofEmployeeApplication(trainingID);

                if(applicationList.Any())
                {
                    //if applicationList < training Capacity
                    if(applicationList.Count() <= selectedTraining.Capacity)
                    {
                        acceptedList.AddRange(applicationList);
                        automaticProcessing.listOfAcceptedEmployees = acceptedList;
                        return automaticProcessing;
                    }
                    else
                    {
                        var matchingDepartmentList = applicationList
                            .Where(employee => employee.DepartmentID == selectedTraining.DepartmentID)
                            .OrderBy(employee => employee.DateRegistered)
                            .Take(selectedTraining.Capacity);

                        var nonMatchingDepartmentList = applicationList
                            .Where(employee => employee.DepartmentID != selectedTraining.DepartmentID)
                            .OrderBy(employee => employee.DateRegistered)
                            .Take(selectedTraining.Capacity - matchingDepartmentList.Count());

                        acceptedList = matchingDepartmentList.Concat(nonMatchingDepartmentList).ToList();
                        rejectedList = applicationList.Except(acceptedList).ToList();
                        automaticProcessing.listOfAcceptedEmployees= acceptedList;
                        automaticProcessing.listOfRejectedEmployees = rejectedList;
                        return automaticProcessing;
                    }
                }
                else
                {
                    return automaticProcessing;
                }
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex);
                return null;
            }
        }
    }
}
