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
        public TrainingService(ITrainingDAL trainingRepo)
        {
            this._trainingRepo = trainingRepo;
        }
        public IEnumerable<ITraining> GetAll()
        {
            var listOfAllTraining= _trainingRepo.GetAll();
            return listOfAllTraining;
        }
    }
}
