using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repo;
using DataLibrary.Repository;
using DataLibrary.Repository.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public class DepartmentService : IDepartmentService
    {


        IDepartmentDAL _departmentDAL;
        public DepartmentService(IDepartmentDAL departmentDAl)
        {
            this._departmentDAL = departmentDAl;
        }
        public IEnumerable<IDepartment> GetAll()
        {
            return _departmentDAL.getDepartments();
        }




    }
}
