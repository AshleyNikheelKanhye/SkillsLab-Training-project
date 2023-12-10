using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Repository.RepoInterfaces
{
    public interface ITrainingDAL
    {
        IEnumerable<ITraining> GetAll();
        IEnumerable<Prerequisite> GetListOfPrerequisites(int trainingID);
    }
}
