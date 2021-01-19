using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

using aiof.messaging.data;
using aiof.messaging.services;

namespace aiof.messaging.function
{
    public class HttpTriggers
    {
        private readonly ILogger<HttpTriggers> _logger;
        private readonly IConfiguration _config;
        private readonly IMessageRepository _repo;

        public HttpTriggers(
            ILogger<HttpTriggers> logger,
            IConfiguration config,
            IMessageRepository repo)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [FunctionName("MessageSend")]
        public async Task<IActionResult> MessageSendAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "message/send")] Message msg)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            await _repo.SendMessageAsync(_config[Keys.InboundQueueName], msg);

            return new OkObjectResult("");
        }
    }
}
