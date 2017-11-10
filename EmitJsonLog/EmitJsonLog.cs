using System;
using RabbitMQ.Client;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Configuration;
using System.IO;

class EmitJsonLog
{
    public static void Main(string[] args)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var factory = new ConnectionFactory() { HostName = $"{config["Hostname"]}" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: $"{config["Exchange"]}", type: "fanout");

            // Send a bunch of messages.
            int NBR_MESSAGES = 100;
            Console.WriteLine("Will send {0} messages", NBR_MESSAGES);
            for (int i = 0; i < NBR_MESSAGES; i++)
            {
                var message = GetMessage(i);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: $"{config["Exchange"]}",
                                     routingKey: "",
                                     basicProperties: null, body: body);
                Console.WriteLine(" [x] Sent {0}", message);
                Thread.Sleep(1000);
            }
        }

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }

    private static string GetMessage(int index)
    {
        return "{ \"data\": \"" + index + "\" }";
    }
}
