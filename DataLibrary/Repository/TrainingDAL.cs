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
                string query = "SELECT * FROM Training";
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

        public IEnumerable<Prerequisite> GetListOfPrerequisites(int trainingID)
        {
            throw new NotImplementedException();
        }

    }
}
