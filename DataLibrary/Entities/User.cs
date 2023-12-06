using DataLibrary.Entities.EntitiesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities
{
    public class User: IUser
    {
        public int UserID { get; set; } 
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string NIC { get; set; }
        public int PhoneNo { get; set; }
        public string Password { get; set; }
        public int DepartmentID { get; set; }
        public int ManagerID { get; set; }
        public string Role { get; set; }

    }
}
