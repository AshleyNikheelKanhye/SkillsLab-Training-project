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
        Task<IEnumerable<IUser>> GetAll();
        Task<IUser> GetById(int userID);
        void Add(RegisterEmployeeViewModel user);
        void Update(IUser user);
        void Delete(int userID);
        IUser Authenticate(LoginUserViewModel loginUserViewModel);
        IEnumerable<ListOfManagersModel> GetAllManagers();
        bool CheckUserExist(CheckUserExistViewModel checkUserExistViewModel);
        IUser Register(RegisterEmployeeViewModel user);
        List<int> GetRoleList(int userId);
        Task<IEnumerable<IUser>> GetEmployeesUnderManager(int managerID);
        Task<int> GetTotalNumberOfUserRecords();
    }
}
