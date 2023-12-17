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

namespace DataLibrary.Repo
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

        public IEnumerable<IDepartment> GetDepartmentsForTraining(int trainingID)
        {
            try
            {
                List<Department> list = new List<Department>(); 
                string selectQuery = "SELECT D.DepartmentID,D.DepartmentName " +
                    "FROM DepartmentTraining DT INNER JOIN Department D ON DT.DepartmentID = D.DepartmentID " +
                    "WHERE DT.TrainingID = @TrainingID";

                SqlCommand command = new SqlCommand(selectQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("TrainingID", trainingID);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    list = DataBaseHelper.ReturnAllRowsFromDB<Department>(reader);
                }
                reader.Close();
                return list;
            }
            catch (Exception ex) { return null; }
        }


    }
}
