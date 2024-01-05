using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities.EntitiesInterface
{
    public interface IRole
    {
        int RoleID { get; set; }
        string RoleName { get; set; }   
    }
}
