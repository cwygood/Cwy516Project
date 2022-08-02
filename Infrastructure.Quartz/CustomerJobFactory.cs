using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Quartz
{
    public class CustomerJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public CustomerJobFactory(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return this._serviceProvider.GetService(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
