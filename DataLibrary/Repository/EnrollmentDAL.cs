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


    }
}
