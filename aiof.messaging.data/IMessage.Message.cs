using System;
using System.Collections.Generic;
using System.Text;

namespace aiof.messaging.data
{
    public interface IEmailMessage
    {
        Guid PublicKey { get; set; }
        string From { get; set; }
        string To { get; set; }
        string Subject { get; set; }
        string Cc { get; set; }
        string Bcc { get; set; }
        bool IsBodyHtml { get; set; }
        string Body { get; set; }

        string ToString();
    }
}