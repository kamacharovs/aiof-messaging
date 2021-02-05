using System;
using System.Text;
using System.Collections.Generic;

namespace aiof.messaging.data
{
    public static class MessageStatus
    {
        public const string Success = nameof(Success);
        public const string Fail = nameof(Fail);
        public const string Received = nameof(Received);
        public const string Retry = nameof(Retry);
    }

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
        public const string DatabaseConnectionString = nameof(DatabaseConnectionString);
        public const string StorageConnectionString = nameof(StorageConnectionString);
        public const string EmailQueueName = nameof(EmailQueueName);
        public const string EmailTableName = nameof(EmailTableName);
        public const string InboundQueueName = nameof(InboundQueueName);
        public const string InboundTableName = nameof(InboundTableName);
        public const string DeadLetterQueueName = nameof(DeadLetterQueueName);
    }

    public static class Entity
    {
        public static string TestConfig = nameof(data.TestConfig).ToSnakeCase();
    }
}
