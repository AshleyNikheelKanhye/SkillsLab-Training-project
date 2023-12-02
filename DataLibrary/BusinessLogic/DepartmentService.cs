using DataLibrary.Entities;
using DataLibrary.Repo;
using DataLibrary.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public class DepartmentService
    {


        DepartmentDAL _departmentDAL;
        public DepartmentService(DepartmentDAL departmentDAl)
        {

            this._departmentDAL = departmentDAl;


        }

        public IEnumerable<Department> GetAll()
        {
            return _departmentDAL.getDepartments();
        }




    }
}
