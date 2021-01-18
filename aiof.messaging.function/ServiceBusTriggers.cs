using System;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

using aiof.messaging.services;

namespace aiof.messaging.function
{
    public class ServiceBusTriggers
    {
        /*private readonly IMessageRepository _repo;

        public ServiceBusTriggers(IMessageRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [FunctionName("ServiceBusInbound")]
        public void Run([ServiceBusTrigger("inbound", Connection = "ServiceBusConnectionString")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
        */
    }
}
