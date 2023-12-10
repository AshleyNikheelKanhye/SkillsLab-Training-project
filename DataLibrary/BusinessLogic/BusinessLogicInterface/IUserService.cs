using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic.BusinessLogicInterface
{
    public interface IUserService
    {
        IEnumerable<IUser> GetAll();
        IUser GetById(int userID);
        void Add(IUser user);
        void Update(IUser user);
        void Delete(int userID);
        IUser Authenticate(LoginUserViewModel loginUserViewModel);
        IEnumerable<ListOfManagersModel> GetAllManagers();
        bool CheckUserExist(CheckUserExistViewModel checkUserExistViewModel);
        IUser Register(User user);
    }
}
