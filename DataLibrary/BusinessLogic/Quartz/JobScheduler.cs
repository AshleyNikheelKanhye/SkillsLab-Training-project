﻿using Quartz.Impl;
using Quartz;
using Unity;
using DataLibrary.BusinessLogic.Logger;
using DataLibrary.Repo;
using DataLibrary.Repository.RepoInterfaces;
using DataLibrary.BusinessLogic.BusinessLogicInterface;
using System.Configuration;
using Unity.Injection;

namespace DataLibrary.BusinessLogic.Quartz
{

    public class JobScheduler
    {
        private static IUnityContainer _container;
        public async static void Start()
        {
            ISchedulerFactory schedularFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedularFactory.GetScheduler();
            scheduler.JobFactory = new MyJobFactory(GetContainer);
            IJobDetail job = JobBuilder.Create<BackgroundJobs>().Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("trigger", "sqlGroup")
             .StartNow()
            .WithDailyTimeIntervalSchedule(x => x
                .WithIntervalInSeconds(50)
                .OnEveryDay())
            .Build();


            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }

        public static IUnityContainer GetContainer
        {
            get
            {
                if (_container == null)
                {
                    _container = new UnityContainer();
                    _container.RegisterType<DBContext>(new InjectionConstructor(ConfigurationManager.ConnectionStrings["default"].ConnectionString));
                    _container.RegisterType<ILogger, DataLibrary.BusinessLogic.Logger.Logger>();
                    _container.RegisterType<ITrainingService, DataLibrary.BusinessLogic.TrainingService>();
                    _container.RegisterType<ITrainingDAL, TrainingDAL>();
                    _container.RegisterType<IUserNotificationDAL, UserNotificationDAL>();
                    _container.RegisterType<IPrerequisiteDAL, PrerequisiteDAL>();
                    _container.RegisterType<IDepartmentDAL, DepartmentDAL>();
                    _container.RegisterType<IQuartzDAL, QuartzDAL>();

                }
                return _container;
            }
        }

    }
}