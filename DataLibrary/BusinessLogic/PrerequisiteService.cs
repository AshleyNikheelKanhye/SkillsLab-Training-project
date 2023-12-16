using DataLibrary.BusinessLogic.BusinessLogicInterface;
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


        public PrerequisiteService(IPrerequisiteDAL prerequisiteRepo ) 
        { 
            this._prerequisiteRepo = prerequisiteRepo;
        }



        public IEnumerable<EmployeeQualificationDetailsViewModel> GetEmployeeQualifications(int userID)   //remember to change <> to EmployeePrerequisite same for BL and controller and interfaces
        {
            try
            {
                return _prerequisiteRepo.GetEmployeeQualifications(userID);
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }

        public bool UploadQualifications(HttpPostedFileBase file, int prerequisiteID, int userID,string fileName)
        {
            return _prerequisiteRepo.UploadQualification(file, prerequisiteID, userID, fileName);
        }

        public EmployeeQualification DownloadQualification(int userID, int prerequisiteID)
        {
            return _prerequisiteRepo.GetQualification(userID, prerequisiteID);
        }

    }
}
