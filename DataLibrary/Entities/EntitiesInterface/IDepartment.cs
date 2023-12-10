using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities.EntitiesInterface
{
    public interface IDepartment
    {
         int DepartmentID { get; set; }
         string DepartmentName { get; set; }
    }
}
