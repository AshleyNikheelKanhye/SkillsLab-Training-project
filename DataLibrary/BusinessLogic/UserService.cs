using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repo;
using DataLibrary.Repository.RepoInterfaces;
using DataLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Services
{
    public class UserService : IUserService
    {

        IUserDAL _userRepo;
        public UserService(IUserDAL userRepo) { 
            this._userRepo = userRepo;
        }

        public void Add(IUser user)
        {
            _userRepo.Add(user);
        }

        public IUser Authenticate(LoginUserViewModel loginUserViewModel)
        {
            IUser user = _userRepo.Find(loginUserViewModel.Email);
            if (user == null)
            {
                return null;
            }

            String givenPwd = loginUserViewModel.Password;
            String obtainedPwd = user.Password;

            if (givenPwd.Equals(obtainedPwd))
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public void Delete(int userID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IUser> GetAll()
        {
            throw new NotImplementedException();
        }

        public IUser GetById(int userID)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<ListOfManagersModel> GetAllManagers()
        {
            return _userRepo.GetAllManagers();
        }

        public void Update(IUser user)
        {
            throw new NotImplementedException();
        }

        public bool CheckUserExist(CheckUserExistViewModel checkUserExistViewModel)
        {
            return _userRepo.CheckUserExists(checkUserExistViewModel.Email, checkUserExistViewModel.NIC, checkUserExistViewModel.PhoneNo);
        }
        public IUser Register(User user)
        {
            return _userRepo.Add(user); 
        }

    }
}
