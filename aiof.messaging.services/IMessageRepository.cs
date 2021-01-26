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

        Task SendMessageAsync<T>(T message)
            where T : IMessage;

        Task SendEmailMessageAsync<T>(T message)
            where T : IEmailMessage;

        Task SendAsync(IMessage message);
    }
}
