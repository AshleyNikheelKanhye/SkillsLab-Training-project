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
        IEnumerable<IEnrollment> GetDeclinedEnrollments(int userID);
        IEnumerable<IEnrollment> GetEnrollments(int UserID,Status FinalStatus, Status ManagerStatus);
        IEnumerable<EnrollmentViewModel> GetPendingEnrollments(int ManagerID);
        Task EmployeeSendMailToManagerForApplication(int userID, int trainingID);
        bool ManagerUpdatesEnrollment(int EnrollmentID,string ManagerResult);
        Task SendTestMail();
        Task ManagerSendMailToEmployee(int EnrollmentID, int ManagerID,string DisapproveMessage);
        IEnumerable<EnrollmentViewModel> GetManagerApproveAndDisapproved(string Choice, int ManagerID);
        Task<IEnumerable<EnrollmentViewModel>> GetEmployeesAppliedForTraining(int trainingID);
    }
}
