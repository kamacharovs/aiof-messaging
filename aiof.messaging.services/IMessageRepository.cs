using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using aiof.messaging.data;

namespace aiof.messaging.services
{
    public interface IMessageRepository
    {
        Task SendInboundMessageAsync(IMessage message);
        Task SendEmailMessageAsync(IEmailMessage message);
        Task SendMessageAsync(
            string queue,
            object message);
        Task SendMessagesAsync(
            string queue,
            IEnumerable<object> messages);
        Task SendAsync(IMessage message);
    }
}
