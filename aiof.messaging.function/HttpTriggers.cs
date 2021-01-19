using System;
using System.IO;
using System.Threading.Tasks;
using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

using FluentValidation;

using aiof.messaging.data;
using aiof.messaging.services;

namespace aiof.messaging.function
{
    public class HttpTriggers
    {
        private readonly IMessageRepository _repo;

        public HttpTriggers(IMessageRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [FunctionName("MessageSend")]
        public async Task<IActionResult> MessageSendAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "message/send")] Message msg)
        {
            try
            {
                await _repo.SendInboundMessageAsync(msg);

                return new OkObjectResult("");
            }
            catch (ValidationException e)
            {
                return new ObjectResult(e.Message)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            catch (Exception e)
            {
                return new ObjectResult(e.Message)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }
    }
}
