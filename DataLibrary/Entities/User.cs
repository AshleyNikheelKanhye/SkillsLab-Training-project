using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.ViewModels;
using System.Collections.Generic;

namespace DataLibrary.Entities
{
    public class User : IUser
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string NIC { get; set; }
        public int PhoneNo { get; set; }
        public string Password { get; set; }
        public int? DepartmentID { get; set; }
        public int? ManagerID { get; set; }
        public string DepartmentName { get; set; }
        public int RoleID {get; set; }
        public List<Role> listOfRoles {  get; set; }
        public int IsActive { get; set; }
        public IEnumerable<EmployeeQualificationDetailsViewModel> listOfQualifications { get; set; }
        public IEnumerable<IEnrollment> listOfTrainingEnrolled { get; set; }
        


    }
}
