using System;

namespace aiof.messaging.data
{
    public static class MessageType
    {
        public const string Email = nameof(Email);
    }

    public static class Keys
    {
        public const string ServiceBusConnectionString = nameof(ServiceBusConnectionString);
        public const string InboundQueueName = nameof(InboundQueueName);
        public const string EmailQueueName = nameof(EmailQueueName);
    }
}
