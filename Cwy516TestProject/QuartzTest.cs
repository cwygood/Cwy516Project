using Infrastructure.Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cwy516TestProject
{
    public class QuartzTest : IClassFixture<CustomTestFixture>
    {
        private CustomTestFixture _factory;
        public QuartzTest(CustomTestFixture factory)
        {
            this._factory = factory;
        }
        [Fact]
        public async void Test()
        {
            var _taskPool = new TaskPoolManager(new StdSchedulerFactory(), _factory.jobFactory);
            await _taskPool.SchedulerJob("ttt", "fff", "0/5 * * * * ?");
            await _taskPool.SchedulerJob("111", "kkk", "0/6 * * * * ?");
        }
    }
}
