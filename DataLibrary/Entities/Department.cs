﻿using DataLibrary.Entities.EntitiesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities
{
    public  class Department : IDepartment
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
    }
}
