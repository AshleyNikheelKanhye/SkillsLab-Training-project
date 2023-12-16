using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities
{
    public class EmployeeQualification
    {
        public int UserID { get; set; }
        public int PrerequisiteID { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }


    }
}
