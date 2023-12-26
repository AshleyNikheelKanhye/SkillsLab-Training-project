using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic.Logger
{
    public interface ILogger
    {
        void Log(string message);
        void LogError(Exception ex);
    }
}
