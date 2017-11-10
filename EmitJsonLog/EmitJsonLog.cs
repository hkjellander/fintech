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
        Console.WriteLine("Press Ctrl+C terminate this program.");

        IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var factory = new ConnectionFactory() { HostName = $"{config["Hostname"]}" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: $"{config["Exchange"]}", type: "fanout");

            Console.WriteLine($"Sending message to exchange '{config["Exchange"]}' " +
                              $"at hostname: {config["Hostname"]}");
            int counter = 0;
            while (true)
            {
                var message = GetMessage(counter++);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: $"{config["Exchange"]}",
                                     routingKey: "",
                                     basicProperties: null, body: body);
                Console.WriteLine(" [x] Sent {0}", message);
                Thread.Sleep(1000);
            }
        }
    }

    private static string GetMessage(int index)
    {
        return "{ \"data\": \"" + index + "\" }";
    }
}
