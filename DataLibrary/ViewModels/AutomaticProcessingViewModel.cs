using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.ViewModels
{
    public class AutomaticProcessingViewModel
    {
        public List<EmployeeApplicationViewModel> listOfAcceptedEmployees { get; set; }
        public List<EmployeeApplicationViewModel> listOfRejectedEmployees { get; set; }
    }
}
