using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repository.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace DataLibrary.Repository
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
                List<IPrerequisite> list = new List<IPrerequisite>();
                string searchQuery = "SELECT p.Details,tp.PrerequisiteID FROM ((Training t INNER JOIN TrainingPrequisite tp ON t.TrainingID = tp.TrainingID) INNER JOIN Prerequisite p ON p.PrerequisiteID = tp.PrerequisiteID) WHERE t.TrainingID=@trainingID";
                SqlCommand command = new SqlCommand(searchQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@trainingID",trainingID);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    int prerequisiteID;
                    string details;
                    while (reader.Read())
                    {
                        prerequisiteID = (int)reader["PrerequisiteID"];
                        details = (string)reader["Details"];
                        list.Add(new Prerequisite() { PrerequisiteID = prerequisiteID, Details = details });
                    }
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
    }
}
