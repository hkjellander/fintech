using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Archive.IntegrationTests.Util
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