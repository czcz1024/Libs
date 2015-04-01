namespace Uninf.Log.Log4Net
{
    public class Log4netConfig:ILog4netConfig
    {
        public Log4netConfig()
        {
            DateFormat = "yyyyMMdd";
            LogFormat = "%date [%thread] %-5level %logger - %message%newline";
        }

        public string GetFileSaveDir()
        {
            return FileSaveDir;
        }

        public string GetDateFormat()
        {
            return DateFormat;
        }

        public string GetLogFormat()
        {
            return LogFormat;
        }

        public string FileSaveDir { get; set; }

        public string DateFormat { get; set; }

        public string LogFormat { get; set; }
    }
}