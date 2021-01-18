using System;
using System.Collections.Generic;
using System.Text;

namespace aiof.messaging.data
{
    public class Message : IMessage
    {
        public MessageType Type { get; set; }
    }
}
