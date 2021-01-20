using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

using Newtonsoft.Json;

namespace aiof.messaging.data
{
    public class TestConfig : ITestConfig
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Guid PublicKey { get; set; } = Guid.NewGuid();

        [Required]
        public string Type { get; set; } = MessageType.Email;

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
