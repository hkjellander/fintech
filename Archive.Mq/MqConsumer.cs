using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.Extensions.Options;

namespace Archive.Mq
{
    public class MqConsumer : IMessageConsumer
    {
        private readonly ILogger _logger;
        private readonly IConnectionFactory _connectionFactory;
        private readonly RabbitMQSettings _config;

        public MqConsumer(ILogger<MqConsumer> logger, IOptions<RabbitMQSettings> config)
        {
            _logger = logger;
            _config = config.Value;
            try
            {
                _connectionFactory = new ConnectionFactory()
                {
                    HostName = _config.Hostname
                };
                _logger.LogInformation("Created MQ connection factory for {0}", _config.Hostname);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in constructor.");
            }
        }

        public void ConsumeMessages()
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: _config.Exchange,
                                            type: "fanout");
                    _logger.LogDebug("Declared exchange '{0}'", _config.Exchange);

                    bool durable = true;
                    bool exclusive = false;
                    bool autoDelete = false;
                    var queue = channel.QueueDeclare(_config.Queue,
                                                     durable, exclusive,
                                                     autoDelete, null);
                    var queueName = queue.QueueName;
                    _logger.LogDebug("Declared queue '{0}'", queueName);

                    channel.QueueBind(queue: queueName,
                                      exchange: _config.Exchange,
                                      routingKey: "");
                    _logger.LogDebug(" [*] Bind to queue '{0}' from exchange '{1}'",
                                     queueName, _config.Exchange);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        _logger.LogDebug(" [x] Received {0}", message);
                    };
                    channel.BasicConsume(queue: queueName, autoAck: true,
                                         consumer: consumer);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during MQ consumption.");
            }
        }
    }
}