﻿using DataLibrary.Repository.RepoInterfaces;
using DataLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities.EntitiesInterface
{
    public interface IUser 
    {
        int UserID { get; set; }
        string Email { get; set; }
        string LastName { get; set; }
        string FirstName { get; set; }
        string NIC { get; set; }
        int PhoneNo { get; set; }
        string Password { get; set; }
        int? DepartmentID { get; set; }
        int? ManagerID { get; set; }
        int RoleID { get; set; }

         List<Role> listOfRoles { get; set; }
         IEnumerable<EmployeeQualificationDetailsViewModel> listOfQualifications { get; set; }
        IEnumerable<IEnrollment> listOfTrainingEnrolled { get; set; }




    }
}
