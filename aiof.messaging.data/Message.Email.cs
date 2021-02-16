using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Azure.Cosmos.Table;

using Newtonsoft.Json;

namespace aiof.messaging.data
{
    /// <summary>
    /// Data transfer object of an email message
    /// </summary>
    public class EmailMessage : IEmailMessage
    {
        public Guid PublicKey { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public bool IsBodyHtml { get; set; } = false;
        public string Body { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    /// <summary>
    /// Email message entity used for data in Azure table storage
    /// </summary>
    public class EmailMessageEntity : TableEntity
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public bool IsBodyHtml { get; set; }
        public string Body { get; set; }
        public string Raw { get; set; }

        public EmailMessageEntity()
        { }

        public EmailMessageEntity(
            string queueName,
            string id)
        {
            PartitionKey = queueName;
            RowKey = id;
        }
    }
}
