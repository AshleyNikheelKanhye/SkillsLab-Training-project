using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repository.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DataLibrary.Repository.DataBaseHelper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Web;
using System.IO;
using DataLibrary.ViewModels;

namespace DataLibrary.Repo
{
    public class PrerequisiteDAL : IPrerequisiteDAL
    {
        DBContext _dbContext;
        public PrerequisiteDAL (DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<IPrerequisite> GetPrerequisites(int trainingID)
        {
            IEnumerable<IPrerequisite> list = new List<IPrerequisite>();
            string searchQuery = "SELECT p.Details,tp.PrerequisiteID FROM ((Training t INNER JOIN TrainingPrequisite tp ON t.TrainingID = tp.TrainingID) INNER JOIN Prerequisite p ON p.PrerequisiteID = tp.PrerequisiteID) WHERE t.TrainingID=@trainingID";
            SqlCommand command = new SqlCommand(searchQuery, _dbContext.GetConn());
            command.Parameters.AddWithValue("@trainingID",trainingID);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                list = DataBaseHelper.ReturnAllRowsFromDB<Prerequisite>(reader);
            }
                
            //If there is not prerequisites the list will be empty
            reader.Close();
            return list;
        }


        public IEnumerable<EmployeeQualificationDetailsViewModel> GetEmployeeQualifications(int userID)
        {
            try
            {
                IEnumerable<EmployeeQualificationDetailsViewModel> list = new List<EmployeeQualificationDetailsViewModel>();
                string selectQuery = "SELECT ep.UserID,ep.PrerequisiteID,ep.FileName,ep.FileContent,p.Details " +
                    "FROM (EmployeePrerequisites ep INNER JOIN Prerequisite p ON ep.PrerequisiteID = p.PrerequisiteID) " +
                    "WHERE ep.UserID =@UserID";
                SqlCommand command = new SqlCommand(selectQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@UserID", userID);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    list = DataBaseHelper.ReturnAllRowsFromDB<EmployeeQualificationDetailsViewModel>(reader);
                }
                //if reader does not has row, the list return will be empty
                reader.Close();
                return list;
            }
            catch{ throw; }
        }

        public async Task<IEnumerable<EmployeeQualificationDetailsViewModel>> GetEmployeeQualificationsDetails(int userID)
        {
            try
            {
                IEnumerable<EmployeeQualificationDetailsViewModel> list = new List<EmployeeQualificationDetailsViewModel>();
                string selectQuery = "SELECT ep.UserID,ep.PrerequisiteID,ep.FileName,p.Details " +
                    "FROM (EmployeePrerequisites ep INNER JOIN Prerequisite p ON ep.PrerequisiteID = p.PrerequisiteID) " +
                    "WHERE ep.UserID =@UserID";
                SqlCommand command = new SqlCommand(selectQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@UserID", userID);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    list = DataBaseHelper.ReturnAllRowsFromDB<EmployeeQualificationDetailsViewModel>(reader);
                }
                //if reader does not has row, the list return will be empty
                reader.Close();
                return list;
            }
            catch { throw; }
        }


        public IEnumerable<EmployeeQualificationDetailsViewModel> GetUserPrerequisiteForEnrollment(int enrollmentID)
        {
            try
            {
                string selectQuery = "SELECT e.UserID,ep.PrerequisiteID,ep.FileName,ep.FileContent,p.Details " +
                                        "FROM ((Enrollment e INNER JOIN EmployeePrerequisites ep ON e.UserID=ep.UserID) " +
                                        "INNER JOIN Prerequisite p ON ep.PrerequisiteID=p.PrerequisiteID) " +
                                        "WHERE e.EnrollmentID = @EnrollmentID AND ep.PrerequisiteID IN " +
                                        "(SELECT tp.PrerequisiteID FROM TrainingPrequisite tp WHERE tp.TrainingID=e.TrainingID)";
                IEnumerable<EmployeeQualificationDetailsViewModel> list = new List<EmployeeQualificationDetailsViewModel>();
                SqlCommand command = new SqlCommand(selectQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@EnrollmentID",enrollmentID );
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    list = DataBaseHelper.ReturnAllRowsFromDB<EmployeeQualificationDetailsViewModel>(reader);
                }
                //if reader does not has row, the list return will be empty
                reader.Close();
                return list;
            }
            catch(Exception ex) { return null; }
        }



        public IEnumerable<IPrerequisite> GetPrerequisitesNotInEmployee(int userID)
        {
            try
            {
                IEnumerable<IPrerequisite> list = new List<IPrerequisite>();
                string selectQuery = "SELECT p.PrerequisiteID, p.Details " +
                                    "FROM (Prerequisite p LEFT JOIN EmployeePrerequisites ep ON p.PrerequisiteID = ep.PrerequisiteID AND ep.UserID = @UserID ) " +
                                    "WHERE ep.PrerequisiteID IS NULL ";

                SqlCommand command = new SqlCommand( selectQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@UserID", userID);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    list = DataBaseHelper.ReturnAllRowsFromDB<Prerequisite>(reader);
                }
                reader.Close();
                return list;
            }
            catch (Exception ex) { return null; }
        }

        public IEnumerable<IPrerequisite> GetAllPrerequisites()
        {
            try
            {
                IEnumerable<IPrerequisite> list = new List<IPrerequisite>();
                string selectQuery = "SELECT * FROM Prerequisite";
                SqlCommand command = new SqlCommand(selectQuery, _dbContext.GetConn());
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    list = DataBaseHelper.ReturnAllRowsFromDB<Prerequisite>(reader);
                }
                reader.Close();
                return list;
            }
            catch { throw; }
        }



        public bool UploadQualification(HttpPostedFileBase file, int prerequisiteID, int userID, string fileName)
        {
            try
            {
                byte[] fileContent;
                using(var binaryReader = new BinaryReader(file.InputStream))
                {
                    fileContent=binaryReader.ReadBytes(file.ContentLength);   
                }
                string insertQuery = "INSERT INTO EmployeePrerequisites (UserID, PrerequisiteID, FileName, FileContent) " +
                                     "VALUES (@UserID, @PrerequisiteID, @FileName, @FileContent)";
                SqlCommand command = new SqlCommand(insertQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@UserID", userID);
                command.Parameters.AddWithValue("@PrerequisiteID", prerequisiteID);
                command.Parameters.AddWithValue("@FileName", fileName);
                command.Parameters.AddWithValue("@FileContent", fileContent);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex) 
            { 
                return false; 
            }
        }


        public EmployeeQualification GetQualification(int userID, int prerequisiteID)
        {
            try
            {
                string selectQuery = "SELECT * FROM EmployeePrerequisites WHERE UserID = @UserID AND PrerequisiteID=@PrerequisiteID";
                SqlCommand command = new SqlCommand(selectQuery, _dbContext.GetConn());
                command.Parameters.AddWithValue("@UserID", userID);
                command.Parameters.AddWithValue("@PrerequisiteID", prerequisiteID);
                SqlDataReader reader = command.ExecuteReader();

                int UserID;
                int PrerequisiteID;
                string FileName;
                byte[] FileContent;

                if(reader.Read())
                {
                    UserID = (int)reader["UserID"];
                    PrerequisiteID = (int)reader["PrerequisiteID"];
                    FileName = (string)reader["FileName"];
                    FileContent = (byte[])reader["FileContent"];
                    EmployeeQualification employeeQualification = new EmployeeQualification()
                    {
                        UserID = UserID,
                        PrerequisiteID = PrerequisiteID,
                        FileName = FileName,
                        FileContent = FileContent
                    };
                    reader.Close();
                    return employeeQualification;
                }
                reader.Close();
                return null;
            }
            catch(Exception ex) { return null; }
        }



    }
}
