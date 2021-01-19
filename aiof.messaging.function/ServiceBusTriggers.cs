using System;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using aiof.messaging.data;
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
        public async Task InboundAsync(
            [ServiceBusTrigger("inbound", Connection = "ServiceBusConnectionString")] string message)
        {
            var msg = JsonConvert.DeserializeObject<Message>(message);

            await _repo.SendAsync(msg);
        }
    }
}
