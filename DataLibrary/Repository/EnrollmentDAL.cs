using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Enum;
using DataLibrary.Repository.DataBaseHelper;
using DataLibrary.Repository.RepoInterfaces;
using DataLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace DataLibrary.Repo
{
    public class EnrollmentDAL :IEnrollmentDAL
    {
        DBContext _dbContext;
        public EnrollmentDAL(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool AddEnrollment(int UserID, int TrainingID)
        {
            try
            {
                string insertQuery = "INSERT INTO Enrollment(UserID,TrainingID) VALUES(@UserID,@TrainingID);";
                SqlCommand command = new SqlCommand(insertQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@UserID", UserID);
                command.Parameters.AddWithValue("@TrainingID", TrainingID);
                int rowsAffected = command.ExecuteNonQuery();
                if(rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }catch { return false; }
        }

        public bool ManagerUpdatesEnrollment(int EnrollmentID,string ManagerResult)
        {
            try
            {
                string updateQuery = "UPDATE Enrollment SET ManagerStatus = @ManagerStatus WHERE EnrollmentID = @EnrollmentID";
                SqlCommand command = new SqlCommand(updateQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@ManagerStatus", ManagerResult);
                command.Parameters.AddWithValue("@EnrollmentID", EnrollmentID);
                int rowsAffected = command.ExecuteNonQuery();
                if(rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetEmployeesAppliedForTraining(int trainingID)
        {
            try
            {
                List<EnrollmentViewModel> list = new List<EnrollmentViewModel>();
                string selectQuery = "SELECT ut.FirstName,ut.LastName,ut.Email,t.TrainingName,t.ClosingDate,t.TrainingStartDate,e.DateRegistered,e.EnrollmentID,e.ManagerStatus,e.FinalStatus " +
                                    "FROM ((Enrollment e INNER JOIN Training t ON e.TrainingID=t.TrainingID) INNER JOIN UserTable ut ON ut.UserID=e.UserID) " +
                                    "WHERE e.TrainingID= @trainingID AND e.IsActive=1 AND ut.IsActive=1 AND t.IsActive=1";
                SqlCommand command = new SqlCommand(selectQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@trainingID",trainingID);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    list = DataBaseHelper.ReturnAllRowsFromDB<EnrollmentViewModel>(reader);
                }
                reader.Close();
                return list;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<EnrollmentViewModel> GetManagerApproveAndDisapproved(string choice,int ManagerID)
        {
            try
            {
                List<EnrollmentViewModel> list = new List<EnrollmentViewModel> ();
                string selectQuery = "SELECT ut.FirstName,ut.LastName,ut.Email,t.TrainingName,t.ClosingDate,t.TrainingStartDate,e.DateRegistered,e.EnrollmentID,e.ManagerStatus,e.FinalStatus " +
                                     "FROM Enrollment e , Training t, UserTable ut " +
                                     "WHERE e.UserID = ut.UserID AND e.TrainingID=t.TrainingID " +
                                     "AND ut.ManagerID=@ManagerID " +
                                     "AND e.ManagerStatus=@Choice " +
                                     "AND e.IsActive =1 AND ut.IsActive =1 AND t.IsActive =1";
                SqlCommand command = new SqlCommand(selectQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@ManagerID", ManagerID);
                command.Parameters.AddWithValue("@Choice", choice);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    list = DataBaseHelper.ReturnAllRowsFromDB<EnrollmentViewModel>(reader);
                }
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<EnrollmentViewModel> GetPendingEnrollments(int ManagerID)
        {
            try
            {
                List<EnrollmentViewModel> list= new List<EnrollmentViewModel>();
                string selectQuery = "SELECT ut.FirstName,ut.LastName,ut.Email,t.TrainingName,t.ClosingDate,t.TrainingStartDate,e.DateRegistered,e.EnrollmentID,e.ManagerStatus,e.FinalStatus " +
                                     "FROM Enrollment e , Training t, UserTable ut " +
                                     "WHERE e.UserID = ut.UserID AND e.TrainingID=t.TrainingID " +
                                     "AND ut.ManagerID=@ManagerID " +
                                     "AND e.ManagerStatus='Processing' " +
                                     "AND t.ClosingDate > GETDATE() " +
                                     "AND e.IsActive =1 AND ut.IsActive =1 AND t.IsActive =1";
                SqlCommand command = new SqlCommand( selectQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@ManagerID",ManagerID.ToString());
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    list = DataBaseHelper.ReturnAllRowsFromDB<EnrollmentViewModel>(reader);
                }
                reader.Close(); 
                return list;
            }
            catch (Exception ex) { return null; }  
        }

        public EnrollmentEmailViewModel GetEnrollmentEmailViewModel(int EnrollmentID)
        {
            try
            {
                EnrollmentEmailViewModel enrollmentEmailViewModel = new EnrollmentEmailViewModel();
                string selectQuery = "SELECT ut.FirstName,ut.LastName,ut.Email,t.TrainingName " +
                                    "FROM ((Enrollment e INNER JOIN UserTable ut ON e.UserID=ut.UserID) INNER JOIN Training t ON e.TrainingID=t.TrainingID) " +
                                    "WHERE e.IsActive=1 AND e.EnrollmentID=@EnrollmentID;";
                SqlCommand command = new SqlCommand(selectQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@EnrollmentID",EnrollmentID);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                     enrollmentEmailViewModel = DataBaseHelper.ReturnSingleRowFromDB<EnrollmentEmailViewModel>(reader);
                }
                reader.Close();
                return enrollmentEmailViewModel;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<IEnrollment> GetEnrollments(int UserID, Status FinalStatus, Status ManagerStatus)
        {
            try
            {
                List<Enrollment> listOfEnrollments=new List<Enrollment>();
                string selectQuery = "SELECT EnrollmentID,e.TrainingID,DateRegistered,ApprovalDate,FinalStatus,ManagerStatus,t.TrainingName,t.ClosingDate,t.TrainingStartDate,t.Capacity,d.DepartmentName " +
                                     "FROM ((Enrollment e INNER JOIN Training t ON e.TrainingID=t.TrainingID) INNER JOIN Department d ON t.DepartmentID = d.DepartmentID) " +
                                     "WHERE FinalStatus =@FinalStatus AND ManagerStatus = @ManagerStatus AND UserID = @UserID AND e.IsActive = 1 AND t.IsActive = 1" ;

                SqlCommand command = new SqlCommand(selectQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@FinalStatus", FinalStatus.ToString());
                command.Parameters.AddWithValue("@ManagerStatus", ManagerStatus.ToString());
                command.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    listOfEnrollments = DataBaseHelper.ReturnAllRowsFromDB<Enrollment>(reader);
                }
                reader.Close();
                return listOfEnrollments;
            }
            catch(Exception ex) { return null; }
        }

    }


}
