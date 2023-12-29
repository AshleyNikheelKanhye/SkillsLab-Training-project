using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.ViewModels;
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
        IEnumerable<ITraining> GetAllElligible(int UserID);
        string GetTrainingName(int trainingID);
        Task<bool> Add(AddTrainingViewModel addTrainingViewModel);
    }
}
