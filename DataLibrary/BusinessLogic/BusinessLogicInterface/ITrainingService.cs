using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repository.RepoInterfaces;
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



    }
}
