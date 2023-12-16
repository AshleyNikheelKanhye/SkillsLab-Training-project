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
            try
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

            }catch (Exception ex)
            {
                return null;
                throw;
            }
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
            catch (Exception ex) { return null; }
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

    }
}
