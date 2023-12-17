using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLibrary.BusinessLogic.BusinessLogicInterface
{
    public interface IPrerequisiteService
    {
        IEnumerable<IPrerequisite> GetPrerequisites(int trainingID);
        IEnumerable<EmployeeQualificationDetailsViewModel> GetEmployeeQualifications(int userID);
        EmployeeQualification DownloadQualification(int userID, int prerequisiteID);
        bool UploadQualifications(HttpPostedFileBase file, int prerequisiteID,int userID,string fileName);
        IEnumerable<IPrerequisite> GetPrerequisitesNotInEmployee(int userID);

    }
}
