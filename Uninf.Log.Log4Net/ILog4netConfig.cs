namespace Uninf.Log.Log4Net
{
    public interface ILog4netConfig
    {
        string GetFileSaveDir();

        string GetDateFormat();

        string GetLogFormat();
    }
}