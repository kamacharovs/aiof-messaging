using System;
using System.Collections.Generic;
using System.Text;

namespace aiof.messaging.data
{
    public interface IMessage
    {
        Guid PublicKey { get; set; }
        string Type { get; set; }
        int? UserId { get; set; }
        DateTime Created { get; set; }

        MessageTestConfig TestConfig { get; set; }

        string From { get; set; }
        string To { get; set; }
        string Subject { get; set; }
        ICollection<string> Cc { get; set; }
        ICollection<string> Bcc { get; set; }
        bool IsBodyHtml { get; set; }
        string Body { get; set; }
    }
}
