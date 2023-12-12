using DataLibrary.Entities.EntitiesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities
{
    public class Prerequisite : IPrerequisite
    {
        public int PrerequisiteID { get; set; }
        public string Details { get; set; }

    }
}
