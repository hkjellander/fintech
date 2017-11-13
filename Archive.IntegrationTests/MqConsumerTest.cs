using Archive.IntegrationTests.Util;
using Archive.Mq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace Archive.IntegrationTests
{
    public class MqConsumerTest
    {
        [Fact]
        public void TestMQ()
        {
            ILogger<MqConsumer> logger = LogUtil<MqConsumer>.GetLogger();
            IOptions<RabbitMQSettings> config = Options.Create(new RabbitMQSettings());

            IMessageConsumer consumer = new MqConsumer(logger, config);
            consumer.ConsumeMessages();
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
