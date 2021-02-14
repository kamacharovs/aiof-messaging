using System;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Azure.Messaging.ServiceBus;

using aiof.messaging.data;
using aiof.messaging.services;

namespace aiof.messaging.function
{
    public class ServiceBusTriggers
    {
        private readonly IMessageRepository _messageRepo;
        private readonly ITableRepository _tableRepo;

        public ServiceBusTriggers(
            IMessageRepository messageRepo,
            ITableRepository tableRepo)
        {
            _messageRepo = messageRepo ?? throw new ArgumentNullException(nameof(messageRepo));
            _tableRepo = tableRepo ?? throw new ArgumentNullException(nameof(tableRepo));
        }

        [FunctionName("Inbound")]
        public async Task InboundAsync(
            [ServiceBusTrigger("%InboundQueueName%", Connection = "ServiceBusConnectionString")] Message message)
        {
            await _messageRepo.SendAsync(message);
        }

        [FunctionName("InboundDeadLetterQueue")]
        public async Task InboundDeadLetterQueueAsync(
            [ServiceBusTrigger("inbound/$DeadLetterQueue", Connection = "ServiceBusConnectionString")] Message message)
        {
            await _tableRepo.LogDeadLetterAsync("inbound", message);
        }
    }
}
