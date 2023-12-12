using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities.EntitiesInterface
{
    public  interface IPrerequisite
    {
         int PrerequisiteID { get; set; }
         string Details { get; set; }
    }
}
