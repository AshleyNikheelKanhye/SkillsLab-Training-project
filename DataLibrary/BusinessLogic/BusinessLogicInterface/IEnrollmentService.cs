using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Enum;
using DataLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic.BusinessLogicInterface
{
    public interface IEnrollmentService
    {
        bool AddEnrollment(int UserID, int TrainingID);
        IEnumerable<IEnrollment> GetEnrollments(int UserID,Status FinalStatus, Status ManagerStatus);
        IEnumerable<ManagerEnrollmentViewModel> GetPendingEnrollments(int ManagerID);
        Task<bool> EmployeeSendMailToManagerForApplication(int userID, int trainingID);
        bool ManagerUpdatesEnrollment(int EnrollmentID,string ManagerResult);
        Task<bool> SendTestMail();
        Task<bool> ManagerSendMailToEmployeeForApproval(int EnrollmentID, int ManagerID);
    }
}
