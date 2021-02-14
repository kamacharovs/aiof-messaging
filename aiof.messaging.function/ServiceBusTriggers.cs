using System;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;

using aiof.messaging.data;
using aiof.messaging.services;

namespace aiof.messaging.function
{
    public class ServiceBusTriggers
    {
        private readonly IMessageRepository _repo;

        public ServiceBusTriggers(IMessageRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [FunctionName("Inbound")]
        public async Task InboundAsync(
            [ServiceBusTrigger("%InboundQueueName%", Connection = "ServiceBusConnectionString")] Message message)
        {
            await _repo.SendAsync(message);
        }
    }
}
