using System;
using System.Collections.Generic;
using System.Text;

namespace aiof.messaging.data
{
    public interface IEmailMessage
    {
        string Bcc { get; set; }
        string Cc { get; set; }
        string From { get; set; }
        string Subject { get; set; }
        string To { get; set; }
    }
}