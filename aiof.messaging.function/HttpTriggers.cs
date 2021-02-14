using System;
using System.IO;
using System.Threading.Tasks;
using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

using aiof.messaging.data;
using aiof.messaging.services;

namespace aiof.messaging.function
{
    public class HttpTriggers
    {
        private readonly ITableRepository _repo;

        public HttpTriggers(ITableRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [FunctionName("MessageSend")]
        [return: ServiceBus("%InboundQueueName%", Connection = "ServiceBusConnectionString")]
        public async Task<IMessage> MessageSend(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "message/send")] Message message)
        {
            await _repo.LogAsync(message);

            return message;
        }
    }
}
