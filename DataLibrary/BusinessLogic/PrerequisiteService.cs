using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.BusinessLogic.Logger;
using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repo;
using DataLibrary.Repository.RepoInterfaces;
using DataLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLibrary.BusinessLogic
{
    public class PrerequisiteService : IPrerequisiteService
    {
        IPrerequisiteDAL _prerequisiteRepo;
        ILogger _logger;

        public PrerequisiteService(IPrerequisiteDAL prerequisiteRepo,ILogger logger ) 
        { 
            this._prerequisiteRepo = prerequisiteRepo;
            this._logger = logger;
        }

        public IEnumerable<EmployeeQualificationDetailsViewModel> GetEmployeeQualifications(int userID)   
        {
            try
            {
                return _prerequisiteRepo.GetEmployeeQualifications(userID);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex);
                return null;
            }
        }

        public IEnumerable<IPrerequisite> GetAllPrerequisites()
        {
            try
            {
                return _prerequisiteRepo.GetAllPrerequisites();
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex);
                return null;
            }
        }

        public IEnumerable<IPrerequisite> GetPrerequisites(int trainingID)
        {
            try
            {
                return _prerequisiteRepo.GetPrerequisites(trainingID);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex);
                return null;
            }
        }

        public IEnumerable<IPrerequisite> GetPrerequisitesNotInEmployee(int userID)
        {
            try
            {
                return _prerequisiteRepo.GetPrerequisitesNotInEmployee(userID);
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex);
                return null;
            }
        }

        public bool UploadQualifications(HttpPostedFileBase file, int prerequisiteID, int userID,string fileName)
        {
            try
            {
                return _prerequisiteRepo.UploadQualification(file, prerequisiteID, userID, fileName);
            }
            catch(Exception ex) 
            {
                this._logger.LogError(ex);
                return false;
            }
        }

        public bool UpdateQualification(HttpPostedFileBase file, int prerequisiteID, int userID, string fileName)
        {
            try
            {
                return _prerequisiteRepo.UpdateQualification(file,prerequisiteID, userID, fileName);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex);
                return false;
            }
        }


        public EmployeeQualification DownloadQualification(int userID, int prerequisiteID)
        {
            try
            {
                return _prerequisiteRepo.GetQualification(userID, prerequisiteID);
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex);
                return null;
            }
        }

        public IEnumerable<EmployeeQualificationDetailsViewModel> GetUserPrerequisiteForEnrollment(int enrollmentID)
        {
            try
            {
                return _prerequisiteRepo.GetUserPrerequisiteForEnrollment(enrollmentID);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex);
                return null;
            }
        }

    }
}
