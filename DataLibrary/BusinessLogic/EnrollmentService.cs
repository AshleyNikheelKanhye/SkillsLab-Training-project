using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.BusinessLogic.Notification;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Enum;
using DataLibrary.Repository.RepoInterfaces;
using DataLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public class EnrollmentService : IEnrollmentService
    {
        IEnrollmentDAL _enrollmentDAL;
        IUserDAL _userDAL;
        ITrainingDAL _trainingDAL;
        public EnrollmentService(IEnrollmentDAL enrollmentDAL,IUserDAL userDAL, ITrainingDAL trainingDAL)
        {
            this._enrollmentDAL = enrollmentDAL;
            this._userDAL = userDAL;
            this._trainingDAL = trainingDAL;
        }

        public bool AddEnrollment(int UserID, int TrainingID)
        {
            return _enrollmentDAL.AddEnrollment(UserID, TrainingID);
        }

        public IEnumerable<IEnrollment> GetEnrollments(int UserID, Status FinalStatus,Status ManagerStatus)
        {
            return _enrollmentDAL.GetEnrollments(UserID,FinalStatus, ManagerStatus);
        }

        public IEnumerable<ManagerEnrollmentViewModel> GetPendingEnrollments(int ManagerID)
        {
            return _enrollmentDAL.GetPendingEnrollments(ManagerID);
        }

        public bool EmployeeSendMailToManagerForApplication(int userID, int trainingID)
        {
            string managerEmail = _userDAL.GetManagerEmailOfEmployee(userID);
            string employeeName = _userDAL.GetFullName(userID);
            string trainingName = _trainingDAL.GetTrainingName(trainingID);



            // build the message body
            string htmlBody = $@"
                <html>
                <head>
                    <title>HTML Email</title>
                </head>
                <body>
                    <p>Hello.</p>
                    <p>Employee <strong>{employeeName}</strong> has applied for Training <strong>{trainingName}</strong></p>
                    <br/>
                    <p>Please take further Action on this application before the deadline.</p>
                </body>
                </html>
            ";

            string subject = $"Training application by {employeeName}";
            try
            {
                bool emailResult = EmailSender.SendEmail(subject, htmlBody, managerEmail);
                return emailResult;
            }
            catch (Exception ex) { throw ex; }
        }

    }
}
