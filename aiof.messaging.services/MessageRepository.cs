using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Azure.ServiceBus;

using aiof.messaging.data;

namespace aiof.messaging.services
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ILogger<MessageRepository> _logger;
        private readonly IQueueClient _queueClient;

        public MessageRepository(
            ILogger<MessageRepository> logger,
            IQueueClient queueClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queueClient = queueClient;
        }

        public void SendAsync(IMessage message)
        {
            switch (message.Type)
            {
                case MessageType.Email:
                    //TODO add validation
                    SendEmailAsync(message);
                    break;
                default:
                    Console.WriteLine("default");
                    break;
            }
        }

        public void SendEmailAsync(IMessage message)
        {
            /*
             * The send email logic would be as follows
             * - receive message, do validation and create the email message
             * - put it in an "email" queue where a Logic App will pick it up and send the actual email
             */
            var cc = string.Join(",", message.Cc);
            var bcc = string.Join(",", message.Bcc);
        }

        public async Task SendMessageAsync()
        {

        }
    }
}
