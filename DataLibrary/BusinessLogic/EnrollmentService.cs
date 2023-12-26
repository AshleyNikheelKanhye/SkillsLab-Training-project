using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.BusinessLogic.Logger;
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
        ILogger _logger;
        public EnrollmentService(IEnrollmentDAL enrollmentDAL,IUserDAL userDAL, ITrainingDAL trainingDAL,ILogger logger)
        {
            this._enrollmentDAL = enrollmentDAL;
            this._userDAL = userDAL;
            this._trainingDAL = trainingDAL;
            this._logger = logger;
        }

        public bool AddEnrollment(int UserID, int TrainingID)
        {
            return _enrollmentDAL.AddEnrollment(UserID, TrainingID);
        }

        public bool ManagerUpdatesEnrollment(int EnrollmentID,string ManagerResult)
        {
            try
            {
                return _enrollmentDAL.ManagerUpdatesEnrollment(EnrollmentID, ManagerResult);
            }
            catch (Exception ex) 
            {
                this._logger.LogError(ex);
                return false;
            }
        }

        public IEnumerable<IEnrollment> GetEnrollments(int UserID, Status FinalStatus,Status ManagerStatus)
        {
            return _enrollmentDAL.GetEnrollments(UserID,FinalStatus, ManagerStatus);
        }

        public IEnumerable<ManagerEnrollmentViewModel> GetPendingEnrollments(int ManagerID)
        {
            return _enrollmentDAL.GetPendingEnrollments(ManagerID);
        }

        public async Task<bool> EmployeeSendMailToManagerForApplication(int userID, int trainingID)
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
                await EmailSender.SendEmail(subject, htmlBody, managerEmail);
                return true;
            }
            catch (Exception ex) 
            {
                this._logger.LogError(ex);
                return false;
            }
        }

        public async Task<bool> ManagerSendMailToEmployeeForApproval(int EnrollmentID , int ManagerID)
        {
            string ManagerName = _userDAL.GetFullName(ManagerID);
            EnrollmentEmailViewModel model = _enrollmentDAL.GetEnrollmentEmailViewModel(EnrollmentID);
            string htmlBody = $@"
                <html>
                <head>
                    <title>Manager Approval</title>
                </head>
                <body>
                    <p>Hello {model.FirstName +" " + model.LastName}</p>
                    <p>Manager <strong>{ManagerName}</strong> has approved you request for Training <strong>{model.TrainingName}</strong></p>
                    <br/>
                </body>
                </html>
            ";

            string subject = $"{ManagerName} approved your request for {model.TrainingName}";
            try
            {
                await EmailSender.SendEmail(subject, htmlBody, model.Email);
                return true;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex);
                return false;
            }

        }



        public async Task<bool> SendTestMail()
        {
            string managerEmail = "lordnk7@gmail.com";
            string htmlBody = $@"
                <html>
                <head>
                    <title>HTML Email</title>
                </head>
                <body>
                    <p>This is a testing email , please ignore </p>
                </body>
                </html>
            ";

            string subject = "this is a test email";
            try
            {
               bool emailResult = await EmailSender.SendEmail(subject, htmlBody, managerEmail);
               return true;
            }
            catch (Exception ex) 
            {
                this._logger.LogError(ex);
                return false;
            }
        }


    }
}
