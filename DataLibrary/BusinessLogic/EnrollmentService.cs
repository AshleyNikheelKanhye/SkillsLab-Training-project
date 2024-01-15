using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.BusinessLogic.Logger;
using DataLibrary.BusinessLogic.Notification;
using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Enum;
using DataLibrary.Repository.RepoInterfaces;
using DataLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        IUserNotificationDAL _userNotificationDAL;
        public EnrollmentService(IEnrollmentDAL enrollmentDAL,IUserDAL userDAL, ITrainingDAL trainingDAL,ILogger logger,IUserNotificationDAL userNotificationDAL)
        {
            this._enrollmentDAL = enrollmentDAL;
            this._userDAL = userDAL;
            this._trainingDAL = trainingDAL;
            this._logger = logger;
            this._userNotificationDAL = userNotificationDAL;
        }

        public bool AddEnrollment(int UserID, int TrainingID)
        {
            try
            {
                _enrollmentDAL.AddEnrollment(UserID, TrainingID);
                InsertNotificationManagerForEmployeeApplication(UserID, TrainingID);
                return true;

            }catch (Exception ex)
            {
                this._logger.LogError(ex);
                return false;
            }
        }


        public void InsertNotificationManagerForEmployeeApplication(int UserID,int TrainingID)
        {
            try
            {
                int managerID = _userDAL.GetManagerIDOfEmployee(UserID);
                string trainingName = _trainingDAL.GetTrainingName(TrainingID);
                string nameOfEmployee = _userDAL.GetFullName(UserID);

                UserNotification notification = new UserNotification()
                {
                    UserID = managerID,
                    Title = "Employee Application For Training",
                    MessageBody = $"{nameOfEmployee} has applied for Training : {trainingName}. Please Make a Decision Before the Training Deadline.",
                };
                _userNotificationDAL.InsertNotification(notification);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex);
                return;
            }
        }

        public bool ManagerUpdatesEnrollment(int EnrollmentID,string ManagerResult)
        {
            try
            {
                _enrollmentDAL.ManagerUpdatesEnrollment(EnrollmentID, ManagerResult);
                InsertNotificationEmployeeForManagerDecision(EnrollmentID, ManagerResult);
                return true;
            }
            catch (Exception ex) 
            {
                this._logger.LogError(ex);
                return false;
            }
        }

        public void InsertNotificationEmployeeForManagerDecision(int EnrollmentID, string ManagerResult)
        {
            try
            {
                EnrollmentEmailViewModel model = _enrollmentDAL.GetEnrollmentEmailViewModel(EnrollmentID);
                UserNotification notification = new UserNotification()
                {
                    UserID = model.UserID,
                    Title = $"Manager Decision For {model.TrainingName}",
                    MessageBody = $"Your Manager has {ManagerResult} your Application for Training {model.TrainingName}",
                };
                _userNotificationDAL.InsertNotification(notification);
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex);
                return;
            }
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetEmployeesAppliedForTraining(int trainingID)
        {
            try
            {
                return await _enrollmentDAL.GetEmployeesAppliedForTraining(trainingID);
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex);
                return null;
            }
        }

        public IEnumerable<IEnrollment> GetEnrollments(int UserID, Status FinalStatus,Status ManagerStatus)
        {
            try
            {
                return _enrollmentDAL.GetEnrollments(UserID,FinalStatus, ManagerStatus);
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex);
                return null;
            }
        }

        public IEnumerable<IEnrollment> GetDeclinedEnrollments(int userID)
        {
            try
            {
                return _enrollmentDAL.GetDeclinedEnrollments(userID);
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex);
                return null;
            }
        }

        public IEnumerable<EnrollmentViewModel> GetPendingEnrollments(int ManagerID)
        {
            try
            {
                return _enrollmentDAL.GetPendingEnrollments(ManagerID);
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex);
                return null;
            }
        }

        public IEnumerable<EnrollmentViewModel> GetManagerApproveAndDisapproved(string Choice,int ManagerID)
        {
            try
            {
                return _enrollmentDAL.GetManagerApproveAndDisapproved(Choice, ManagerID);
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex);
                return null;
            }
        }

        public async Task EmployeeSendMailToManagerForApplication(int userID, int trainingID)
        {
            string managerEmail = _userDAL.GetManagerEmailOfEmployee(userID);
            string employeeName = _userDAL.GetFullName(userID);
            string trainingName = _trainingDAL.GetTrainingName(trainingID);

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
                return ;
            }
            catch (Exception ex) 
            {
                this._logger.LogError(ex);
                return ;
            }
        }

        public async Task ManagerSendMailToEmployee(int EnrollmentID , int ManagerID,string DisapprovalMessage)
        {
            try
            {
                string htmlBody , subject;
                string ManagerName = _userDAL.GetFullName(ManagerID);
                EnrollmentEmailViewModel model = _enrollmentDAL.GetEnrollmentEmailViewModel(EnrollmentID);

                if (DisapprovalMessage == "") //meaning approval
                {
                     htmlBody = $@"
                    <html>
                    <head>
                        <title>Manager Approval</title>
                    </head>
                    <body>
                        <p>Hello {model.FirstName + " " + model.LastName}</p>
                        <p>Manager <strong>{ManagerName}</strong> has approved you request for Training <strong>{model.TrainingName}</strong></p>
                        <br/>
                    </body>
                    </html>
                    ";

                     subject = $"{ManagerName} approved your request for {model.TrainingName}";
                }
                else //meaning disaproval
                {
                     htmlBody = $@"
                    <html>
                    <head>
                        <title>Manager Disapproval</title>
                    </head>
                    <body>
                        <p>Hello {model.FirstName + " " + model.LastName}</p>
                        <p>Manager <strong>{ManagerName}</strong> has Disapproved you request for Training <strong>{model.TrainingName}</strong></p>
                        <p>Reason : {DisapprovalMessage}</p>
                        <br/>
                    </body>
                    </html>
                    ";

                     subject = $"{ManagerName} Disaproved your request for {model.TrainingName}";
                }
                await EmailSender.SendEmail(subject, htmlBody, model.Email);
                return ;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex);
                return ;
            }

        }
    }
}
