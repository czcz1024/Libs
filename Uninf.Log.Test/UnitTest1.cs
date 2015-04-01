using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Uninf.Log.Test
{
    using System.Threading.Tasks;

    using log4net;
    using log4net.Appender;
    using log4net.Layout;
    using log4net.Repository.Hierarchy;

    using Uninf.Log.Log4Net;
    using Uninf.Log.NLog;

    [TestClass]
    public class UnitTest1
    {
        static string basepath = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin")) + @"\logs\";
        [TestMethod]
        public void TestMethod1()
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.RemoveAllAppenders(); /*Remove any other appenders*/

            RollingFileAppender fileAppender = new RollingFileAppender();
            fileAppender.AppendToFile = true;
            fileAppender.LockingModel = new FileAppender.MinimalLock();
            fileAppender.File =basepath;
            fileAppender.DatePattern = "yyyyMMdd.TXT";
            fileAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
            fileAppender.StaticLogFileName = false;
            PatternLayout pl = new PatternLayout();

            pl.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            pl.ActivateOptions();
            fileAppender.Layout = pl;
            fileAppender.ActivateOptions();

            log4net.Config.BasicConfigurator.Configure(fileAppender);

            //Test logger
            for (var i = 0; i < 5; i++)
            {
                new Task(
                    () =>
                        {
                            var log = LogManager.GetLogger("TestFile");
                            log.Debug("Testing!");
                        }).Start();
            }
        }

        [TestMethod]
        public void 日志测试()
        {
            var logger1 = new TestLogger(new TestConfig());
            logger1.Error("err");
            for (var i = 0; i < 5; i++)
            {
                new Task(
                    () =>
                        {
                            var logger = new TestLogger(new TestConfig());
                            logger.Error("err");
                        }).Start();
            }
        }

        [TestMethod]
        public void Nlog测试()
        {
            var config = new NLogConfig {
                FileSaveDir = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin")) + @"\logs\"
            };
            var logger = new NLogConfigLogger(config);
            logger.Error("err");
        }
    }

    public class TestLogger:Log4netConfigLogger
    {
        public TestLogger(ILog4netConfig config)
            : base(config)
        {
        }
    }

    public class TestConfig : Log4netConfig
    {
        public TestConfig()
        {
            this.FileSaveDir =  AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin")) + @"\logs\";;
        }
    }
}
