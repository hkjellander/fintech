using Microsoft.Extensions.Logging;

namespace Archive.Test.Common
{
    public class LogUtil<T>
    {
        public static ILogger<T> GetLogger()
        {
            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddConsole(LogLevel.Trace);
            return loggerFactory.CreateLogger<T>();
        }
    }
}