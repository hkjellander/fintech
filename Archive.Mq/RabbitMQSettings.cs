using System;
namespace Archive.Mq
{
    public class RabbitMQSettings
    {
        public RabbitMQSettings() {
            Hostname = "localhost";
            Exchange = "logs";
            Queue = "log-archive-queue";
        }
        public string Hostname { get; set; }
        public string Exchange { get; set; }
        public string Queue { get; set; }
    }
}
