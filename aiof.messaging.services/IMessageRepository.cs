using System;
using System.Collections.Generic;
using System.Text;

using aiof.messaging.data;

namespace aiof.messaging.services
{
    public interface IMessageRepository
    {
        void SendAsync(IMessage message);
    }
}
