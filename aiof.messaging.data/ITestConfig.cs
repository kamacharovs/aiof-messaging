using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace aiof.messaging.data
{
    public interface ITestConfig
    {
        [Required]
        int Id { get; set; }

        [Required]
        Guid PublicKey { get; set; }

        [Required]
        string Type { get; set; }

        string PhoneNumber { get; set; }
        string Email { get; set; }
        string Subject { get; set; }
    }
}