using DataLibrary.Repository.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Repo
{
    public class QuartzDAL :IQuartzDAL
    {
        DBContext _dbContext;
        public QuartzDAL(DBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task InsertQuartzJobLog(string jobName)
        {
            string insertQuery = @"INSERT INTO QuartzJobLogs(JobDescription) VALUES (@jobDescription); ";
            SqlCommand command = new SqlCommand(insertQuery, _dbContext.GetConn());
            command.Parameters.AddWithValue("@jobDescription", jobName);
            await command.ExecuteNonQueryAsync();
        }

    }
}
