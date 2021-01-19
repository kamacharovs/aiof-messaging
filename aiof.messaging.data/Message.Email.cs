using System;
using System.Collections.Generic;
using System.Text;

namespace aiof.messaging.data
{
    public class EmailMessage : IEmailMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
    }
}
