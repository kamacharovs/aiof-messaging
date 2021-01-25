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

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
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
    public class MessageEntity : TableEntity
    {
        public Guid PublicKey { get; set; }
        public string Type { get; set; }
        public int? UserId { get; set; }
        public DateTime Created { get; set; }


        public MessageEntity()
        { }

        public MessageEntity(string queueName, string id)
        {
            PartitionKey = queueName;
            RowKey = id;
        }
    }
}
