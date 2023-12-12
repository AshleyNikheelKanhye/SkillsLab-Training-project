using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repository.RepoInterfaces;
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
        public TrainingService(ITrainingDAL trainingRepo,IPrerequisiteDAL prerequisiteRepo)
        {
            this._trainingRepo = trainingRepo;
            this._prerequisiteDAL = prerequisiteRepo;
            
        }
        public IEnumerable<ITraining> GetAll()
        {
            var listOfAllTraining= _trainingRepo.GetAll();
            return listOfAllTraining;
        }

        public IEnumerable<IPrerequisite> GetPrerequisites(int trainingID)
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


    }
}
