using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repository.DataBaseHelper;
using DataLibrary.Repository.RepoInterfaces;
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
            string query = "SELECT * FROM Training";
            SqlCommand command = new SqlCommand(query,_dbContext.GetConn());
            SqlDataReader reader = command.ExecuteReader();

            List<Training> returnList = DataBaseHelper.ReturnAllRowsFromDB<Training>(reader);
            return returnList;

        }
    }
}
