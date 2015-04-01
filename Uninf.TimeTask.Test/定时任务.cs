using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Uninf.TimeTask.Test
{
    using System.Collections.Generic;
    using System.Threading;

    using Quartz;
    using Quartz.Impl;

    using Uninf.TimeTask.QuartZ;

    [TestClass]
    public class 定时任务
    {
        [TestMethod]
        public void TestMethod1()
        {
            var server = new Server();
            server.StartAll();

            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            var gnames = scheduler.GetJobGroupNames();
            Thread.Sleep(2000);

            Assert.AreEqual(1, TestTask.X);
        }
    }

    public class TestTask:QuartZTimeTaskBase
    {
        private Dp dp;

        public TestTask(Dp dp)
        {
            this.dp = dp;
        }

        public static int X = 0;
        public override void Exec()
        {
            X = 1;
        }

        public override string RunTime()
        {
            return "0/1 * * * * ?";
        }
    }

    public class Dp
    {
    }

    public class Server:QuartZServer
    {
        protected override IEnumerable<IQuartZJob> GetJobs()
        {
            return new[] { new TestTask(new Dp()) };
        }
    }
}
