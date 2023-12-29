using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLibrary.Repository.RepoInterfaces
{
    public interface IPrerequisiteDAL
    {
        IEnumerable<IPrerequisite> GetPrerequisites(int trainingID);
        IEnumerable<EmployeeQualificationDetailsViewModel> GetEmployeeQualifications(int userID);

        bool UploadQualification(HttpPostedFileBase file, int prerequisiteID, int userID, string fileName);
        EmployeeQualification GetQualification(int userID, int prerequisiteID);
        IEnumerable<IPrerequisite> GetPrerequisitesNotInEmployee(int userID);
        IEnumerable<EmployeeQualificationDetailsViewModel> GetUserPrerequisiteForEnrollment(int enrollmentID);
        IEnumerable<IPrerequisite> GetAllPrerequisites();
    }
}
