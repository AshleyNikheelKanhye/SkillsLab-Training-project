using Quartz.Spi;
using Quartz;
using System;
using Unity;

namespace DataLibrary.BusinessLogic.Quartz
{
    public class MyJobFactory : IJobFactory
    {
        private readonly IUnityContainer _container;
        public MyJobFactory(IUnityContainer container)
        {
            _container = container;
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler) => (IJob)_container.Resolve(bundle.JobDetail.JobType);

        public void ReturnJob(IJob job)
        {
            IDisposable disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
