using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class ReceiveLogs
{
    public static int Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.Error.WriteLine("Usage: {0} <id>", Environment.GetCommandLineArgs()[0]);
            Console.Error.WriteLine("Where the id argument is mandatory.");
            return 1;
        }
        var id = args[0];
        var factory = new ConnectionFactory() { 
            HostName = "localhost",
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "logs",
                                    type: "fanout");
            bool durable = true;
            bool exclusive = false;
            bool autoDelete = false;
            var queue = channel.QueueDeclare("my-queue-" + id,
                                             durable, exclusive,
                                             autoDelete, null);
                                        
            var queueName = queue.QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: "logs",
                              routingKey: "my-key");

            Console.WriteLine(" [*] ID: {0} Waiting for messages.", id);

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
