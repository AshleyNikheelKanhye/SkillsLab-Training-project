using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repository.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DataLibrary.Repository.DataBaseHelper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace DataLibrary.Repo
{
    public class PrerequisiteDAL : IPrerequisiteDAL
    {
        DBContext _dbContext;
        public PrerequisiteDAL (DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<IPrerequisite> GetPrerequisites(int trainingID)
        {
            try
            {
                IEnumerable<IPrerequisite> list = new List<IPrerequisite>();
                string searchQuery = "SELECT p.Details,tp.PrerequisiteID FROM ((Training t INNER JOIN TrainingPrequisite tp ON t.TrainingID = tp.TrainingID) INNER JOIN Prerequisite p ON p.PrerequisiteID = tp.PrerequisiteID) WHERE t.TrainingID=@trainingID";
                SqlCommand command = new SqlCommand(searchQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@trainingID",trainingID);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    list = DataBaseHelper.ReturnAllRowsFromDB<Prerequisite>(reader);
                }
                
                //If there is not prerequisites the list will be empty
                reader.Close();
                return list;

            }catch (Exception ex)
            {
                return null;
                throw;
            }
        }


        public IEnumerable<IPrerequisite> GetEmployeeQualifications(int userID)
        {
            try
            {
                IEnumerable<IPrerequisite> list = new List<IPrerequisite>();
                string selectQuery = "SELECT p.PrerequisiteID,p.Details " +
                    "FROM ((UserTable u INNER JOIN EmployeePrerequisites ep ON u.UserID=ep.UserID)" +
                    " INNER JOIN Prerequisite p ON p.PrerequisiteID=ep.PrerequisiteID) " +
                    "WHERE u.UserID =@UserID";
                SqlCommand command = new SqlCommand(selectQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@UserID", userID);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    list = DataBaseHelper.ReturnAllRowsFromDB<Prerequisite>(reader);
                }
                //if reader does not has row, the list return will be empty
                reader.Close();
                return list;
            }
            catch (Exception ex) { return null; }
        }
    }
}
