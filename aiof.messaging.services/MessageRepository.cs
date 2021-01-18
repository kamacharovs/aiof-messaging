using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using aiof.messaging.data;

namespace aiof.messaging.services
{
    public class MessageRepository : IMessageRepository
    {
        public ILogger<MessageRepository> _logger;

        public MessageRepository(
            ILogger<MessageRepository> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void SendAsync()
        {

        }
    }
}
