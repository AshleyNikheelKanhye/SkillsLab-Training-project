using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.Entities;
using DataLibrary.Repository.RepoInterfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic.Quartz
{
    public class BackgroundJobs : IJob
    {
        private readonly IUserNotificationService _userNotificationService;
        public BackgroundJobs(IUserNotificationService userNotificationService)
        {
            _userNotificationService = userNotificationService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
             await _userNotificationService.InsertDummyNotification();


        }
    }
}
