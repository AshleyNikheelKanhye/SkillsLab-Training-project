using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Repository.RepoInterfaces
{
    public interface IPrerequisiteDAL
    {
        IEnumerable<IPrerequisite> GetPrerequisites(int trainingID);
        IEnumerable<IPrerequisite> GetEmployeeQualifications(int userID);
    }
}
