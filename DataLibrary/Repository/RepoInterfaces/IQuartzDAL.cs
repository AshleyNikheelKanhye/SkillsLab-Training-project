using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Repository.RepoInterfaces
{
    public interface IQuartzDAL
    {
        Task InsertQuartzJobLog(string jobName);
    }
}
