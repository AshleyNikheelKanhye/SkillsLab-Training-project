using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repo;
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

        UserDAL _userRepo;
        public UserService(UserDAL userRepo) { 
            
            this._userRepo = userRepo;
            
        
        }

        public void Add(IUser user)
        {
            _userRepo.Add(user);
        }

        public IUser Authenticate(LoginUserViewModel loginUserViewModel)
        {
            
            
                User user = (User)_userRepo.Find(loginUserViewModel.Email);


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

        public void Update(IUser user)
        {
            throw new NotImplementedException();
        }
    }
}
