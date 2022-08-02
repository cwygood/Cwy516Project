using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Quartz.Listeners
{
    public class TriggerListener : ITriggerListener
    {
        public string Name
        {
            get { return "Test"; }
        }

        public Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("TriggerComplete......");
            return Task.CompletedTask;
        }

        public Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("TriggerFired......");
            return Task.CompletedTask;
        }

        public Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("TriggerMisfired......");
            return Task.CompletedTask;
        }
        /// <summary>
        /// 哪些任务可以不需要执行下一个调度
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("VetoJobExecution......");
            return Task.FromResult(false);
        }
    }
}
