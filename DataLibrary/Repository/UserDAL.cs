﻿using DataLibrary.Entities;
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

        public bool Add(IUser user)
        {
            string insertQuery = "INSERT INTO [dbo].[UserTable] (FirstName,LastName,Password,Email,NIC,PhoneNo,Role,DepartmentID,ManagerID) VALUES " +
                 "(@FirstName,@LastName,@Password,@Email,@NIC,@PhoneNo,@Role,@DepartmentID,@ManagerID);";
            try
            {
                SqlCommand cmd = new SqlCommand(insertQuery, _dbContext.GetConn());

                cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                cmd.Parameters.AddWithValue("@LastName", user.LastName);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@NIC", user.NIC);
                cmd.Parameters.AddWithValue("@PhoneNo", user.PhoneNo);
                cmd.Parameters.AddWithValue("@Role", user.Role);
                cmd.Parameters.AddWithValue("@DepartmentID", user.DepartmentID);
                cmd.Parameters.AddWithValue("@ManagerID", user.ManagerID);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }

        public bool CheckUserExists(string Email, string NIC, int PhoneNo)
        {
            string searchQuery = "SELECT Count(UserID) FROM UserTable where Email = @Email OR NIC = @NIC OR PhoneNo = @PhoneNo ";
            SqlCommand cmd = new SqlCommand(searchQuery, _dbContext.GetConn());
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@NIC", NIC);
            cmd.Parameters.AddWithValue("@PhoneNo", PhoneNo);
            int rowCount = (int)cmd.ExecuteScalar();
            if(rowCount > 0)
            {
                return true;
            }
            else { return false; }

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
