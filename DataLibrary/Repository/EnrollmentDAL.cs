using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Enum;
using DataLibrary.Repository.DataBaseHelper;
using DataLibrary.Repository.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            }catch (Exception ex) { return false; }
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
