using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repo;
using DataLibrary.Repository.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public class PrerequisiteService : IPrerequisiteService
    {
        IPrerequisiteDAL _prerequisiteRepo;


        public PrerequisiteService(IPrerequisiteDAL prerequisiteRepo ) 
        { 
            this._prerequisiteRepo = prerequisiteRepo;
        }



        public IEnumerable<IPrerequisite> GetEmployeeQualifications(int userID)
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
    }
}
