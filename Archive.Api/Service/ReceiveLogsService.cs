using Archive.Mq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace Archive.Api.Service
{
    class ReceiveLogsService : HostedService
    {
        private readonly IMessageConsumer _messageConsumer;
        private readonly ILogger _logger;

        public ReceiveLogsService(IMessageConsumer messageConsumer,
                                  ILogger<ReceiveLogsService> logger)
        {
            _messageConsumer = messageConsumer;
            _logger = logger;
            _logger.LogInformation("Starting ReceiveLogsService");
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting ReceiveLogsService.ExecuteAsync");
            while (!cancellationToken.IsCancellationRequested)
            {
                // TODO(hekj): Should the below method be async as well?
                _messageConsumer.ConsumeMessages();
                _logger.LogDebug("Sleeping for 5 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }
    }
}
