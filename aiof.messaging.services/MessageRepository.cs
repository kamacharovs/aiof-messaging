using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Azure.Messaging.ServiceBus;

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
            var cc = string.Join(",", message.Cc);
            var bcc = string.Join(",", message.Bcc);

            //await SendEmailMessageAsync();
        }

        public async Task SendEmailMessageAsync()
        {
            // create a sender for the queue 
            var sender = _client.CreateSender(_config[Keys.EmailQueueName]);

            // create a message that we can send
            var message = new ServiceBusMessage("Hello world!");

            // send the message
            await sender.SendMessageAsync(message);
            Console.WriteLine($"Sent a single message to the queue: {_config[Keys.EmailQueueName]}");
        }
    }
}
