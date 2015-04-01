namespace Uninf.Log.Log4Net
{
    using log4net;
    using log4net.Appender;
    using log4net.Core;
    using log4net.Layout;
    using log4net.Repository.Hierarchy;

    using ILog = Uninf.Log.ILogger;

    public class Log4netConfigLogger:Log4netLogger
    {
        private ILog4netConfig config;

        public Log4netConfigLogger(ILog4netConfig config)
        {
            this.config = config;
            logger=base.GetLogger();
        }

        protected override log4net.ILog GetLogger()
        {
            if (config != null)
            {
                LogManager.GetRepository().ResetConfiguration();
                var hierarchy = (Hierarchy)LogManager.GetRepository();
                hierarchy.Root.RemoveAllAppenders();

                var fileAppender = new RollingFileAppender();
                fileAppender.AppendToFile = true;
                fileAppender.LockingModel = new FileAppender.MinimalLock();
                fileAppender.File = config.GetFileSaveDir();
                fileAppender.DatePattern = config.GetDateFormat() + ".TXT";
                fileAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
                fileAppender.StaticLogFileName = false;

                var pl = new PatternLayout();
                pl.ConversionPattern = config.GetLogFormat();
                pl.ActivateOptions();
                fileAppender.Layout = pl;
                fileAppender.ActivateOptions();

                log4net.Config.BasicConfigurator.Configure(fileAppender);
            }
            return base.GetLogger();
        }
    }
}