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
        private readonly ITrainingService _trainingService;
        public BackgroundJobs(ITrainingService trainingService)
        {
            _trainingService = trainingService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
             await _trainingService.QuartzAutomaticProcessing();
        }
    }
}
