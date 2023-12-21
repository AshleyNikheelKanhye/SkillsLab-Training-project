using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.Repository.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public class EnrollmentService : IEnrollmentService
    {
        IEnrollmentDAL _enrollmentDAL;
        public EnrollmentService(IEnrollmentDAL enrollmentDAL)
        {
            this._enrollmentDAL = enrollmentDAL;
        }

        public bool AddEnrollment(int UserID, int TrainingID)
        {
            return _enrollmentDAL.AddEnrollment(UserID, TrainingID);
        }



    }
}
