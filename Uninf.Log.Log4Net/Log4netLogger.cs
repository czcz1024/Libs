namespace Uninf.Log.Log4Net
{
    using log4net;

    public class Log4netLogger:ILogger
    {
        protected ILog logger;

        public Log4netLogger()
        {
            
        }

        public void Error(string msg)
        {
            GetLogger().Error(msg);
        }

        public void Debug(string msg)
        {
            GetLogger().Debug(msg);
        }

        public void Info(string msg)
        {
            GetLogger().Info(msg);
        }

        private static readonly object locker = new object();
        protected virtual ILog GetLogger()
        {
            if (logger == null)
            {
                lock (locker)
                {
                    if (logger == null)
                    {
                        logger = LogManager.GetLogger("default");
                    }
                }
            }
            return logger;
        }
    }
}