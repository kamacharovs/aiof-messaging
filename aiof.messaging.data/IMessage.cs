﻿using System;
using System.Collections.Generic;
using System.Text;

namespace aiof.messaging.data
{
    public interface IMessage
    {
        MessageType Type { get; set; }
    }
}
