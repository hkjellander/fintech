using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IO;

class ReceiveLogs
{
    public static int Main(string[] args)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .Build();

        var factory = new ConnectionFactory() { HostName = $"{config["Hostname"]}" };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: $"{config["Exchange"]}",
                                    type: "fanout");
            bool durable = true;
            bool exclusive = false;
            bool autoDelete = false;
            var queue = channel.QueueDeclare($"{config["Queue"]}",
                                             durable, exclusive,
                                             autoDelete, null);

            var queueName = queue.QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: $"{config["Exchange"]}",
                              routingKey: "");

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] {0}", message);
            };
            channel.BasicConsume(queue: queueName, autoAck: true,
                                 consumer: consumer);
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
            return 0;
        }
    }
}
