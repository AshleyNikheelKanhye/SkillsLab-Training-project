using DataLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Repository.DataBaseHelper;

namespace DataLibrary.Repository
{
    public class DepartmentDAL
    {


        DBContext _dbContext;
        public DepartmentDAL(DBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IEnumerable<Department> getDepartments()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM Department", _dbContext.GetConn());
            SqlDataReader reader = command.ExecuteReader();


            List<Department> list = new List<Department>();
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



        }






    }
}
