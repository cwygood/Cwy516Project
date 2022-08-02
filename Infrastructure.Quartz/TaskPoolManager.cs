using Domain.Interfaces;
using Infrastructure.Common.Mq;
using Infrastructure.Configurations;
using Infrastructure.Quartz.Listeners;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Quartz
{
    public class TaskPoolManager
    {
        private readonly IScheduler _scheduler;
        public TaskPoolManager(ISchedulerFactory schedulerFactory, IJobFactory customerJobFactory)
        {
            this._scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            //NameValueCollection properties = new NameValueCollection();
            //properties["quartz.threadPool.threadCount"] = "50";
            //properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            //properties["quartz.scheduler.exporter.port"] = "555";
            //properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            //properties["quartz.scheduler.exporter.channelType"] = "tcp";
            //ISchedulerFactory sf = new StdSchedulerFactory(properties);
            this._scheduler.JobFactory = customerJobFactory;
            this._scheduler.ListenerManager.AddSchedulerListener(new SchedulerListener());
            this._scheduler.ListenerManager.AddJobListener(new JobListener());
            this._scheduler.ListenerManager.AddTriggerListener(new TriggerListener());
            _scheduler.Start().GetAwaiter().GetResult();
        }

        public async Task<DateTimeOffset?> SchedulerJob(string name, string group, string cron)
        {
            var jobKey = new JobKey(name, group);
            var trigger = TriggerBuilder.Create()
                .WithIdentity(jobKey.Name, jobKey.Group)
                .StartNow()
                .WithCronSchedule(cron)
                .WithDescription(name)
                .Build();
            var nextFireTime = trigger.GetFireTimeAfter(DateTimeOffset.UtcNow - TimeSpan.FromSeconds(1));
            if (!nextFireTime.HasValue)
            {
                throw new Exception("任务调度失败：执行频率设置不正确，导致任务永远不会被执行，请检查执行频率参数！");
            }
            if(await _scheduler.CheckExists(jobKey))
            {
                return await _scheduler.RescheduleJob(trigger.Key, trigger);
            }
            JobDataMap jobMap = new JobDataMap();
            jobMap.Put("Test", name);
            var job = JobBuilder.Create<Job>()
                .WithIdentity(jobKey)
                .UsingJobData(jobMap)
                .WithDescription(name)
                .Build();
            return await _scheduler.ScheduleJob(job, trigger);
        } 
    }
}
