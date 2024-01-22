using DataLibrary.BusinessLogic.Logger;
using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repository.DataBaseHelper;
using DataLibrary.Repository.RepoInterfaces;
using DataLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Repo
{
    public class UserNotificationDAL : IUserNotificationDAL
    {
        DBContext _dbContext;
        

        public UserNotificationDAL(DBContext dbContext)
        {
            _dbContext = dbContext;
            
        }

        public async Task<IEnumerable<IUserNotification>> GetUserNotifications(int userID)
        {

            List<UserNotification> list = new List<UserNotification>();
            string selectQuery = @"SELECT * FROM UserNotification WHERE UserID=@userid ORDER BY MessageDate DESC";
            SqlCommand command = new SqlCommand(selectQuery, _dbContext.GetConn());
            command.Parameters.AddWithValue("@userid",userID);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                list = DataBaseHelper.ReturnAllRowsFromDB<UserNotification>(reader);
            }
            reader.Close();
            return list;
        }

        public async Task InsertDummyNotification() //testing purposes
        {
            try
            {
                string insertQuery = "INSERT INTO UserNotification(UserID,Title,MessageBody) VALUES(1006,'QUARTZ','this is a test QUARTZ')";
                SqlCommand command = new SqlCommand(insertQuery, _dbContext.GetConn());
                await command.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertNotification(UserNotification usernotification)
        {
            try
            {
                string insertQuery = "INSERT INTO UserNotification(UserID,Title,MessageBody) VALUES(@managerID,@title,@messageBody)";
                SqlCommand command = new SqlCommand(insertQuery,_dbContext.GetConn());
                command.Parameters.AddWithValue("@managerID", usernotification.UserID);
                command.Parameters.AddWithValue("@title", usernotification.Title);
                command.Parameters.AddWithValue("@messageBody", usernotification.MessageBody);
                command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }
    }
}
