using System;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

using aiof.messaging.services;

namespace aiof.messaging.function
{
    public class ServiceBusTriggers
    {
        private readonly ILogger<ServiceBusTriggers> _logger;
        private readonly IMessageRepository _repo;

        public ServiceBusTriggers(
            ILogger<ServiceBusTriggers> logger,
            IMessageRepository repo)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [FunctionName("Inbound")]
        public void Run([ServiceBusTrigger("inbound", Connection = "ServiceBusConnectionString")] string myQueueItem)
        {
            _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var k = 1;
        }
    }
}
