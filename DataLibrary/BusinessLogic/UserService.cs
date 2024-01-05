using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.BusinessLogic.Logger;
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
        ILogger _logger;
        IPrerequisiteDAL _prerequisiteDAL;
        IEnrollmentDAL _enrollmentDAL;
        public UserService(IUserDAL userRepo,ILogger logger, IPrerequisiteDAL prerequisiteDAl, IEnrollmentDAL enrollmentDAL)
        {
            this._userRepo = userRepo;
            this._logger = logger;
            this._prerequisiteDAL = prerequisiteDAl;
            this._enrollmentDAL = enrollmentDAL;
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
            //TODO : Add hashing for passwords
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

        public List<int> GetRoleList(int userId)
        {
            return _userRepo.GetRoleList(userId);
        }



        public IEnumerable<ListOfManagersModel> GetAllManagers()
        {
            return _userRepo.GetAllManagers();
        }

        public bool CheckUserExist(CheckUserExistViewModel checkUserExistViewModel)
        {
            return _userRepo.CheckUserExists(checkUserExistViewModel.Email, checkUserExistViewModel.NIC, checkUserExistViewModel.PhoneNo);
        }
        public IUser Register(User user)
        {
            return _userRepo.Add(user); 
        }

        public void Delete(int userID)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<IUser>> GetAll()
        {
            try
            {
                return await _userRepo.GetAll();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex);
                return null;
            }
        }
        public async Task<int> GetTotalNumberOfUserRecords()
        {
            try
            {
                return await _userRepo.GetTotalNumberOfUserRecords();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex);
                return -1;
            }
        }

        public async Task<IUser> GetById(int userID)
        {
            try
            {
                IUser user =  await _userRepo.GetById(userID);
                if(user != null)
                {
                    user.listOfQualifications = await _prerequisiteDAL.GetEmployeeQualificationsDetails(userID);
                    user.listOfRoles = _userRepo.GetRoles(userID);
                    user.listOfTrainingEnrolled =  await _enrollmentDAL.GetUserEnrollments(userID);
                }
                return user;    
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex);
                return null;
            }
        }
        public void Update(IUser user)
        {
            throw new NotImplementedException();
        }
    }
}
