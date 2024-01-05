using DataLibrary.Entities.EntitiesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities
{
    public class Role:IRole
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }
}
