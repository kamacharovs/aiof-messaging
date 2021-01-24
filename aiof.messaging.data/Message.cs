using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Azure.Cosmos.Table;

using Newtonsoft.Json;

namespace aiof.messaging.data
{
    /// <summary>
    /// Data transfer object of a message. Based on the type, lower level logic is performed and the message is send appropriately
    /// </summary>
    public class Message : IMessage
    {
        public Guid PublicKey { get; set; } = Guid.NewGuid();
        public string Type { get; set; }
        public int? UserId { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;

        /*
         * Test
         */
        public MessageTestConfig TestConfig { get; set; }

        /*
         * Email
         */
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public ICollection<string> Cc { get; set; } = new List<string>();
        public ICollection<string> Bcc { get; set; } = new List<string>();
    }

    /// <summary>
    /// Message test configuration
    /// </summary>
    public class MessageTestConfig
    {
        public bool? IsTest { get; set; }
        public bool? UseConfig { get; set; }
        public int? Id { get; set; }
    }

    /// <summary>
    /// Message entity used to manipulate data in Azure table storage
    /// </summary>
    public class MessageInboundEntity : TableEntity
    {
        public string ContentDataRaw { get; set; }
        public string ContentData { get; set; }
        public string Description { get; set; }


        public MessageInboundEntity()
        { }

        public MessageInboundEntity(string id)
        {
            PartitionKey = Keys.InboundQueueName;
            RowKey = id;
        }
    }
}
