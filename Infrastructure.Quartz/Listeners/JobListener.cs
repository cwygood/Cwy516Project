using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Quartz.Listeners
{
    public class JobListener : IJobListener
    {
        public string Name
        {
            get { return "Test"; }
        }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("JobExecutionVetoed......");
            return Task.CompletedTask;
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("JobToBeExecuted......");
            return Task.CompletedTask;
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("JobWasExecuted......");
            return Task.CompletedTask;
        }
    }
}
