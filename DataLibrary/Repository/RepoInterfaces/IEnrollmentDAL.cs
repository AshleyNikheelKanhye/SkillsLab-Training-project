using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Enum;
using DataLibrary.ViewModels;
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
        IEnumerable<ManagerEnrollmentViewModel> GetPendingEnrollments(int ManagerID);
        bool ManagerUpdatesEnrollment(int EnrollmentID,string ManagerResult);
        EnrollmentEmailViewModel GetEnrollmentEmailViewModel(int EnrollmentID);
        IEnumerable<ManagerEnrollmentViewModel> GetManagerApproveAndDisapproved(string choice, int ManagerID);

    }
}
