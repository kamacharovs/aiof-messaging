using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace aiof.messaging.data
{
    /// <summary>
    /// Data transfer object of a message. Based on the type, lower level logic is performed and the message is send appropriately
    /// </summary>
    public class Message : IMessage
    {
        public Guid PublicKey { get; set; } = Guid.NewGuid();
        public MessageType Type { get; set; }
        public int? UserId { get; set; }
        public bool? IsTest { get; set; }

        /*
         * Email
         */
        public string From { get; set; }
        public string To { get; set; }
        public ICollection<string> Cc { get; set; } = new List<string>();
        public ICollection<string> Bcc { get; set; } = new List<string>();
    }
}
