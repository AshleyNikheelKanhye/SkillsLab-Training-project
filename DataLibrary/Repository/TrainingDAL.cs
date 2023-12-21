using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repository.DataBaseHelper;
using DataLibrary.Repository.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


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
            { //need to add check in query for if the trainingID selected is not already enrolled with userid in Enrollment ( just add another check constraint where tid not in enrollment.tid)
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

    }
}
