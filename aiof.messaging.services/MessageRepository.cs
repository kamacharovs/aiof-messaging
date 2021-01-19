using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Azure.Messaging.ServiceBus;

using Newtonsoft.Json;
using AutoMapper;

using aiof.messaging.data;

namespace aiof.messaging.services
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ILogger<MessageRepository> _logger;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ServiceBusClient _client;

        public MessageRepository(
            ILogger<MessageRepository> logger,
            IConfiguration config,
            IMapper mapper,
            ServiceBusClient client)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task SendMessageAsync(
            string queue,
            object message)
        {
            var msgStr = JsonConvert.SerializeObject(message);

            try
            {
                var sender = _client.CreateSender(queue);
                var sbMessage = new ServiceBusMessage(msgStr);

                await sender.SendMessageAsync(sbMessage);

                _logger.LogInformation("Sent message={message} to queue={queue}",
                    msgStr,
                    queue);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while sending {queue} message", queue);
            }
        }

        public async Task SendAsync(IMessage message)
        {
            switch (message.Type)
            {
                case MessageType.Email:
                    //TODO add validation
                    await SendEmailAsync(message);
                    break;
                default:
                    Console.WriteLine("default");
                    break;
            }
        }

        public async Task SendEmailAsync(IMessage message)
        {
            /*
             * The send email logic would be as follows
             * - receive message, do validation and create the email message
             * - put it in an "email" queue where a Logic App will pick it up and send the actual email
             */
            var emailMsg = _mapper.Map<IEmailMessage>(message);

            await SendMessageAsync(
                _config[Keys.EmailQueueName], 
                emailMsg);
        }
    }
}
