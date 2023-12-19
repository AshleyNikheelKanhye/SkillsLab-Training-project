using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Repository.RepoInterfaces
{
    public interface IUserDAL
    {
        IEnumerable<IUser> GetAll();
        IEnumerable<ListOfManagersModel> GetAllManagers();
        IUser GetById(int userID);
        IUser Add(IUser user);
        void Update(IUser user);
        void Delete(int userID);
        IUser Find(string email);
        bool CheckUserExists(string Email, string NIC, int PhoneNo);
        List<int> GetRoleList(int userID);


    }
}
