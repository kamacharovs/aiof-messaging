﻿using System;
using System.Text;
using System.Collections.Generic;

namespace aiof.messaging.data
{
    public static class MessageType
    {
        public const string Email = nameof(Email);
        public const string Sms = nameof(Sms);

        public static IEnumerable<string> All => new List<string>
        {
            Email,
            Sms
        };
    }

    public static class Keys
    {
        public const string ServiceBusConnectionString = nameof(ServiceBusConnectionString);
        public const string InboundQueueName = nameof(InboundQueueName);
        public const string EmailQueueName = nameof(EmailQueueName);
    }
}
