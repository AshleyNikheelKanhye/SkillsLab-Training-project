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
    public class UserDAL : IUserDAL
    {
        DBContext _dbContext;
        public UserDAL(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(IUser user)
        {
            string insertQuery = "INSERT INTO [dbo].[UserTable] (FirstName,LastName,Password,Email,NIC,PhoneNo,Role) VALUES " +
                 "(@FirstName,@LastName,@Password,@Email,@NIC,@MobileNumber,@Role);";

            SqlCommand cmd = new SqlCommand(insertQuery, _dbContext.GetConn());

            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@NIC", user.NIC);
            cmd.Parameters.AddWithValue("@MobileNumber", user.PhoneNo);
            cmd.Parameters.AddWithValue("@Role", user.Role);

            cmd.ExecuteNonQuery();

        }

        public void Delete(int userID)
        {
            throw new NotImplementedException();
        }

        public IUser Find(string email)
        {
                SqlCommand command = new SqlCommand("SELECT UserID,Email,FirstName,LastName,NIC,PhoneNo,Password,Role FROM [UserTable] WHERE Email = @Email", _dbContext.GetConn());
                command.Parameters.AddWithValue("@Email", email);
                SqlDataReader reader = command.ExecuteReader();

            /*                while (reader.Read())
                            {
                                user = new User()
                                {
                                    Id = reader.GetInt32(0),
                                    Email = reader.GetString(1),
                                    FirstName = reader.GetString(2),
                                    LastName = reader.GetString(3),
                                    NIC = reader.GetString(4),
                                    PhoneNo = reader.GetInt32(5),
                                    Password = reader.GetString(6),
                                    Role = reader.GetString(7),
                                };
                            }*/

            if (reader.Read())
            {
                return  DataBaseHelper.MapUserFromReaders<User>(reader);   
            }
                reader.Close();
                return null;
        }


        

        public IEnumerable<IUser> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ListOfManagersModel> GetAllManagers()
        {
            List<ListOfManagersModel> list = new List<ListOfManagersModel>();

            SqlCommand command = new SqlCommand("SELECT UserID,FirstName,LastName FROM UserTable WHERE Role = @Role", _dbContext.GetConn());
            command.Parameters.AddWithValue("@Role", "manager"); // TODO: change that to enum later
            SqlDataReader reader = command.ExecuteReader();

            int userID;
            string firstname;
            string lastname;

            while (reader.Read())
            {
                userID = (int)reader["UserID"];
                firstname = (string)reader["FirstName"];
                lastname= (string)reader["LastName"];
                list.Add(new ListOfManagersModel() {  UserID= userID , FirstName = firstname , LastName=lastname});
            }
            reader.Close();
            return list;
        }

        public IUser GetById(int userID)
        {
            throw new NotImplementedException();
        }

        public void Update(IUser user)
        {
            throw new NotImplementedException();
        }
    }
}
