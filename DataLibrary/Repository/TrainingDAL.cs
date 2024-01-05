using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Enum;
using DataLibrary.Repository.DataBaseHelper;
using DataLibrary.Repository.RepoInterfaces;
using DataLibrary.ViewModels;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace DataLibrary.Repo
{
    public class TrainingDAL : ITrainingDAL
    {
        DBContext _dbContext;
        public TrainingDAL(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<ITraining> GetAll()
        {
            try
            {
                List<Training> returnList = new List<Training>();
                string query = "SELECT TrainingID,TrainingName,Capacity,ClosingDate,TrainingStartDate,t.DepartmentID,d.DepartmentName " +
                                "FROM Training t INNER JOIN Department d ON t.DepartmentID = d.DepartmentID ";
                                
                SqlCommand command = new SqlCommand(query,_dbContext.GetConn());
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    returnList = DataBaseHelper.ReturnAllRowsFromDB<Training>(reader);
                }
                reader.Close();
                return returnList;

            }catch (Exception ex) { return null; }
        }

        public async Task<IEnumerable<ITraining>> getUpcomings()
        {
            try
            {
                List<Training> returnList = new List<Training>();
                string selectQuery = "SELECT TrainingID,TrainingName,Capacity,ClosingDate,TrainingStartDate,t.DepartmentID,d.DepartmentName,Duration,Description,IsAutomaticProcessed " +
                                "FROM Training t INNER JOIN Department d ON t.DepartmentID = d.DepartmentID " +
                                "WHERE t.ClosingDate > GETDATE() AND t.IsActive =1 " +
                                "ORDER BY t.ClosingDate ASC;";
                SqlCommand command = new SqlCommand(selectQuery,_dbContext.GetConn());
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    returnList = DataBaseHelper.ReturnAllRowsFromDB<Training>(reader);
                }
                reader.Close();
                return returnList;
            }
            catch { throw; }
        }

        public IEnumerable<ITraining> GetAllElligible(int UserID)
        {
            try
            { 
                List<Training> returnList = new List<Training>();
                string query = "SELECT t.TrainingID,t.TrainingName,t.Capacity,t.ClosingDate,t.TrainingStartDate,t.DepartmentID,d.DepartmentName " +
                                "FROM Training t INNER JOIN Department d ON t.DepartmentID = d.DepartmentID " +
                                "WHERE t.IsActive=1 AND t.ClosingDate>GETDATE() AND t.TrainingID IN" +
                                                                                " ( SELECT tp.TrainingID " +
                                                                                   "FROM TrainingPrequisite tp JOIN EmployeePrerequisites ep ON tp.PrerequisiteID = ep.PrerequisiteID " +
                                                                                   "AND ep.UserID = @UserID " +
                                                                                   "GROUP BY tp.TrainingID " +
                                                                                   "HAVING COUNT(DISTINCT tp.PrerequisiteID) = " +
                                                                                                                                "(SELECT COUNT(DISTINCT PrerequisiteID) " +
                                                                                                                                "FROM TrainingPrequisite " +
                                                                                                                                "WHERE TrainingID = tp.TrainingID))" +
                                                                                                                                " AND t.TrainingID NOT IN (SELECT e.TrainingID" +
                                                                                                                                                            " FROM Enrollment e" +
                                                                                                                                                            " WHERE e.UserID = @UserID)";
                SqlCommand command = new SqlCommand(query, _dbContext.GetConn());
                command.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    returnList = DataBaseHelper.ReturnAllRowsFromDB<Training>(reader);
                }
                reader.Close();
                return returnList;
            }catch { return null; }   
        }



        public IEnumerable<Prerequisite> GetListOfPrerequisites(int trainingID)
        {
            throw new NotImplementedException();
        }

        public string GetTrainingName(int trainingID)
        {
            try
            {
                string TrainingName = "";
                string selectQuery = " SELECT TrainingName FROM Training WHERE TrainingID = @trainingID ";
                SqlCommand command = new SqlCommand(selectQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@trainingID", trainingID);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    TrainingName = (string)reader["TrainingName"];
                }
                reader.Close();
                return TrainingName;
            }
            catch (Exception ex) { throw ex; }
        }


        public async Task<bool> Add(AddTrainingViewModel addTrainingViewModel)
        {
            try
            {
                string insertQuery = "INSERT INTO Training(TrainingName,Capacity,ClosingDate,TrainingStartDate,DepartmentID) " +
                                     "OUTPUT INSERTED.TrainingID "+
                                     "VALUES (@trainingName,@capacity,@closingDate,@trainingStartDate,@departmentID);";

                SqlCommand command = new SqlCommand(insertQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@trainingName", addTrainingViewModel.TrainingName);
                command.Parameters.AddWithValue("@capacity", addTrainingViewModel.TrainingCapacity);
                command.Parameters.AddWithValue("@closingDate", addTrainingViewModel.DeadlineRegistration);
                command.Parameters.AddWithValue("@trainingStartDate", addTrainingViewModel.StartingDate);
                command.Parameters.AddWithValue("@departmentID", addTrainingViewModel.Department);

                int insertedTrainingID=-1;
                using(SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if(reader.Read())
                    {
                        insertedTrainingID = reader.GetInt32(0);
                    }
                }
                
                if (insertedTrainingID >0)
                {
                    //now need to update table TrainingPrerequisites
                    string insertQueryForTrainingPrerequisite = "INSERT INTO TrainingPrequisite VALUES (@trainingID,@prerequisiteID)";
                    foreach(int prerequisiteID in addTrainingViewModel.PrerequisiteList)
                    {
                        SqlCommand command2 = new SqlCommand(insertQueryForTrainingPrerequisite, _dbContext.GetConn());
                        command2.Parameters.AddWithValue("@trainingID", insertedTrainingID);
                        command2.Parameters.AddWithValue("@prerequisiteID", prerequisiteID);
                        await command2.ExecuteNonQueryAsync();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch { throw; }
        }

        public async Task<IEnumerable<ITraining>> GetUnprocessedTrainings()
        {
            try
            {
                List<Training> returnList = new List<Training>();
                string selectQuery = "SELECT TrainingID,TrainingName,Capacity,ClosingDate,TrainingStartDate,t.DepartmentID,d.DepartmentName " +
                                    "FROM Training t INNER JOIN Department d ON t.DepartmentID = d.DepartmentID " +
                                    "WHERE IsAutomaticProcessed = 0 AND t.IsActive =1 ";

                SqlCommand command = new SqlCommand(selectQuery, _dbContext.GetConn());
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    returnList = DataBaseHelper.ReturnAllRowsFromDB<Training>(reader);
                }
                reader.Close();
                return returnList;
            }catch { throw; }
        }

        public async Task<ITraining> GetTraining(int trainingID)
        {
            try
            {
                Training trainingReturnModel = new Training();
                string selectQuery = "SELECT TrainingID,TrainingName,Capacity,ClosingDate,TrainingStartDate,t.DepartmentID,d.DepartmentName " +
                                    "FROM Training t INNER JOIN Department d ON t.DepartmentID = d.DepartmentID " +
                                    "WHERE t.TrainingID = @trainingID";
                SqlCommand command = new SqlCommand(selectQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@trainingID", trainingID);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                {
                    trainingReturnModel = DataBaseHelper.ReturnSingleRowFromDB<Training>(reader);
                }
                reader.Close();
                return trainingReturnModel;
            }
            catch { throw; }
        }





        public async Task<List<EmployeeApplicationViewModel>> GetListofEmployeeApplication(int trainingID)
        {
            try
            {
                List<EmployeeApplicationViewModel> list = new List<EmployeeApplicationViewModel>();
                string selectQuery = "Select ut.FirstName,ut.LastName,ut.UserID,ut.Email,d.DepartmentID,d.DepartmentName,e.EnrollmentID,e.DateRegistered,e.ManagerStatus,e.FinalStatus,t.TrainingName,t.TrainingStartDate " +
                                     "FROM Enrollment e, Training t, UserTable ut,Department d " +
                                     "WHERE e.TrainingID=t.TrainingID AND e.UserID=ut.UserID AND ut.DepartmentID=d.DepartmentID " +
                                     "AND e.TrainingID = @trainingID AND e.ManagerStatus='Approved' AND e.IsActive=1 AND t.IsAutomaticProcessed=0";
                SqlCommand command = new SqlCommand( selectQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@trainingID", trainingID);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if(reader.HasRows)
                {
                    list = DataBaseHelper.ReturnAllRowsFromDB<EmployeeApplicationViewModel>(reader);    
                }
                reader.Close();
                return list;
            }
            catch { throw; }
        }

        public async Task<bool> ConfirmAutomaticSelection(AutomaticProcessingViewModel trainingSelectionResult,int trainingID)
        {
            SqlTransaction transaction = _dbContext.GetConn().BeginTransaction();
            try
            {
                await UpdateEnrollmentTable(trainingSelectionResult,transaction);
                await UpdateTrainingTable(trainingID,transaction);
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

        }

        public async Task<bool> UpdateEnrollmentTable(AutomaticProcessingViewModel trainingSelectionResult,SqlTransaction transaction)
        {
            try
            {
                List<EmployeeApplicationViewModel> listAccepted = trainingSelectionResult.listOfAcceptedEmployees;
                List<EmployeeApplicationViewModel> listRejected = trainingSelectionResult.listOfRejectedEmployees;
                string updateQueryApprove = "UPDATE Enrollment SET FinalStatus = '"+Status.Approved.ToString()+"' WHERE EnrollmentID=@EnrollmentIDApprove";
                string updateQueryDisapprove = "UPDATE Enrollment SET FinalStatus = '" + Status.Disapproved.ToString() + "' WHERE EnrollmentID=@EnrollmentIDDisapprove";

                //approved Enrollments

                foreach (EmployeeApplicationViewModel application in listAccepted)
                {
                    using (SqlCommand approveCommand = new SqlCommand(updateQueryApprove, _dbContext.GetConn(), transaction))
                    {
                        SqlParameter enrollmentIDApproved = new SqlParameter("@EnrollmentIDApprove", System.Data.SqlDbType.Int);
                        enrollmentIDApproved.Value = application.EnrollmentID;
                        approveCommand.Parameters.Add(enrollmentIDApproved);
                        await approveCommand.ExecuteNonQueryAsync();
                    }
                }

                //disapproved Enrollments
                foreach (EmployeeApplicationViewModel application in listRejected)
                {
                    using (SqlCommand disapproveCommand = new SqlCommand(updateQueryDisapprove, _dbContext.GetConn(), transaction))
                    {
                        SqlParameter enrollmentIDDisapproved = new SqlParameter("@EnrollmentIDDisapprove", System.Data.SqlDbType.Int);
                        enrollmentIDDisapproved.Value = application.EnrollmentID;
                        disapproveCommand.Parameters.Add(enrollmentIDDisapproved);
                        await disapproveCommand.ExecuteNonQueryAsync();
                    }
                }
                return true;
            }
            catch { throw; }
        }

        public async Task<bool> UpdateTrainingTable(int trainingID,SqlTransaction transaction)
        {
            try
            {
                string updateQuery = "UPDATE Training SET IsAutomaticProcessed=1 WHERE TrainingID = @trainingID";
                SqlCommand command = new SqlCommand(updateQuery,_dbContext.GetConn(), transaction);
                command.Parameters.AddWithValue("@trainingID", trainingID);
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch { throw; }
        }


    }
}
