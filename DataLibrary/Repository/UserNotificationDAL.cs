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
            string selectQuery = @"SELECT * FROM UserNotification WHERE UserID=@userid";
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


    }
}
