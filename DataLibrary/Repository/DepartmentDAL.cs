using DataLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Repository.DataBaseHelper;
using DataLibrary.Repository.RepoInterfaces;
using DataLibrary.Entities.EntitiesInterface;

namespace DataLibrary.Repository
{
    public class DepartmentDAL : IDepartmentDAL
    {
        DBContext _dbContext;
        public DepartmentDAL(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<IDepartment> getDepartments()
        {
            try
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Department", _dbContext.GetConn());
                SqlDataReader reader = command.ExecuteReader();
                List<IDepartment> list = new List<IDepartment>();
                int deptID;
                string deptDetails;
                while (reader.Read()) 
                {
                    deptID = (int)reader["DepartmentID"];
                    deptDetails = (string)reader["DepartmentName"];
                    list.Add(new Department() { DepartmentID=deptID,DepartmentName=deptDetails});
                }
                reader.Close();
                return list;

            }catch (Exception ex) { return null; }
        }
    }
}
