using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using aiof.messaging.data;
using aiof.messaging.services;

namespace aiof.messaging.function
{
    public class HttpTriggers
    {
        private readonly ILogger<HttpTriggers> _logger;
        private readonly IMessageRepository _repo;

        public HttpTriggers(
            ILogger<HttpTriggers> logger,
            IMessageRepository repo)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [FunctionName("MessageSend")]
        public async Task<IActionResult> MessageSendAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "message/send")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var msg = JsonConvert.DeserializeObject<Message>(requestBody);

            await _repo.SendAsync(msg);

            return new OkObjectResult(msg);
        }
    }
}
