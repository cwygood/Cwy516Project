using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Quartz.Listeners
{
    public class SchedulerListener : ISchedulerListener
    {
        public Task JobAdded(IJobDetail jobDetail, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("JobAdded......");
            return Task.CompletedTask;
        }

        public Task JobDeleted(JobKey jobKey, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("JobDeleted......");
            return Task.CompletedTask;
        }

        public Task JobInterrupted(JobKey jobKey, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("JobInterrupted......");
            return Task.CompletedTask;
        }

        public Task JobPaused(JobKey jobKey, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("JobPaused......");
            return Task.CompletedTask;
        }

        public Task JobResumed(JobKey jobKey, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("JobResumed......");
            return Task.CompletedTask;
        }

        public Task JobScheduled(ITrigger trigger, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("JobScheduled......");
            return Task.CompletedTask;
        }

        public Task JobsPaused(string jobGroup, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("JobsPaused......");
            return Task.CompletedTask;
        }

        public Task JobsResumed(string jobGroup, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("JobsResumed......");
            return Task.CompletedTask;
        }

        public Task JobUnscheduled(TriggerKey triggerKey, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("JobUnscheduled......");
            return Task.CompletedTask;
        }

        public Task SchedulerError(string msg, SchedulerException cause, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("SchedulerError......");
            return Task.CompletedTask;
        }

        public Task SchedulerInStandbyMode(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("SchedulerInStandbyMode......");
            return Task.CompletedTask;
        }

        public Task SchedulerShutdown(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("SchedulerShutdown......");
            return Task.CompletedTask;
        }

        public Task SchedulerShuttingdown(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("SchedulerShuttingdown......");
            return Task.CompletedTask;
        }

        public Task SchedulerStarted(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("SchedulerStarted......");
            return Task.CompletedTask;
        }

        public Task SchedulerStarting(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("SchedulerStarting......");
            return Task.CompletedTask;
        }

        public Task SchedulingDataCleared(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("SchedulingDataCleared......");
            return Task.CompletedTask;
        }

        public Task TriggerFinalized(ITrigger trigger, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("TriggerFinalized......");
            return Task.CompletedTask;
        }

        public Task TriggerPaused(TriggerKey triggerKey, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("TriggerPaused......");
            return Task.CompletedTask;
        }

        public Task TriggerResumed(TriggerKey triggerKey, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("TriggerResumed......");
            return Task.CompletedTask;
        }

        public Task TriggersPaused(string triggerGroup, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("TriggersPaused......");
            return Task.CompletedTask;
        }

        public Task TriggersResumed(string triggerGroup, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("TriggersResumed......");
            return Task.CompletedTask;
        }
    }
}
