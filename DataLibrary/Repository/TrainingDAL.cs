using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repository.DataBaseHelper;
using DataLibrary.Repository.RepoInterfaces;
using DataLibrary.ViewModels;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

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
                                "FROM Training t INNER JOIN Department d ON t.DepartmentID = d.DepartmentID";
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
            }catch(Exception ex) { return null; }   
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

    }
}
