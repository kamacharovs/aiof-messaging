using System;
using System.Collections.Generic;
using System.Text;

using static aiof.messaging.data.Constants;

namespace aiof.messaging.data
{
    public interface IMessage
    {
        MessageType Type { get; set; }
    }
}
