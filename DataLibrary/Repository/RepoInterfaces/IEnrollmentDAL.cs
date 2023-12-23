using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Repository.RepoInterfaces
{
    public interface IEnrollmentDAL
    {
        bool AddEnrollment(int UserID, int TrainingID);
        IEnumerable<IEnrollment> GetEnrollments(int UserID, Status FinalStatus, Status ManagerStatus);

    }
}
