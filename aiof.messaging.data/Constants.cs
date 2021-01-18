using System;

namespace aiof.messaging.data
{
    public enum MessageType
    {
        Email
    }

    public static class Keys
    {
        public const string ServiceBusConnectionString = nameof(ServiceBusConnectionString);
    }
}
