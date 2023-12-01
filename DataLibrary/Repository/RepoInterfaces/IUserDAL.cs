using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
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
        IUser GetById(int userID);
        void Add(IUser user);
        void Update(IUser user);
        void Delete(int userID);

        IUser Find(string email);



    }
}
