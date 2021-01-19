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
        bool? IsTest { get; set; }
        DateTime Created { get; set; }

        string From { get; set; }
        string To { get; set; }
        string Subject { get; set; }
        ICollection<string> Cc { get; set; }
        ICollection<string> Bcc { get; set; }
    }
}
