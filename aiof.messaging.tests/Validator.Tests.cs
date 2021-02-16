using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using FluentValidation;

using aiof.messaging.data;

namespace aiof.messaging.tests
{
    [Trait(Helper.Category, Helper.UnitTest)]
    public class ValidatorTests
    {
        private readonly AbstractValidator<IMessage> _messageValidator;
        private readonly AbstractValidator<IEmailMessage> _emailMessageValidator;

        public ValidatorTests()
        {
            _messageValidator = Helper.GetRequiredService<AbstractValidator<IMessage>>() ?? throw new ArgumentNullException(nameof(AbstractValidator<IMessage>));
            _emailMessageValidator = Helper.GetRequiredService<AbstractValidator<IEmailMessage>>() ?? throw new ArgumentNullException(nameof(AbstractValidator<IEmailMessage>));
        }

        [Theory]
        [InlineData(MessageType.Email)]
        [InlineData(MessageType.Sms)]
        public void Message_Validate_Type_IsSuccessful(string type)
        {
            var message = new Message
            {
                Type = type
            };

            Assert.True(_messageValidator.Validate(message).IsValid);
        }

        [Theory]
        [InlineData("somerandomtype")]
        [InlineData("definitelydoesntexit")]
        [InlineData("")]
        [InlineData(null)]
        public void Message_Validate_Type_IsInvalid(string type)
        {
            var message = new Message
            {
                Type = type
            };

            Assert.False(_messageValidator.Validate(message).IsValid);
        }

        [Theory]
        [InlineData("finance@aiof.com", "finance2@aiof.com")]
        [InlineData("finance2@aiof.com", "finance@aiof.com")]
        public void MessageEmail_IsSuccessful(string from, string to)
        {
            var emailMessage = new EmailMessage
            {
                From = from,
                To = to,
                Subject = "This is a test subject",
                Body = "This is a test"
            };

            Assert.True(_emailMessageValidator.Validate(emailMessage).IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("12")]
        [InlineData("notanemail")]
        public void MessageEmail_From_IsInvalid(string from)
        {
            var emailMessage = new EmailMessage
            {
                From = from,
                To = "finance2@aiof.com",
                Subject = "This is a test subject"
            };

            Assert.False(_emailMessageValidator.Validate(emailMessage).IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("12")]
        [InlineData("notanemail")]
        public void MessageEmail_To_IsInvalid(string to)
        {
            var emailMessage = new EmailMessage
            {
                From = "finance@aiof.com",
                To = to,
                Subject = "This is a test subject"
            };

            Assert.False(_emailMessageValidator.Validate(emailMessage).IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void MessageEmail_Subject_IsInvalid(string subject)
        {
            var emailMessage = new EmailMessage
            {
                From = "finance@aiof.com",
                To = "finance2@aiof.com",
                Subject = subject
            };

            Assert.False(_emailMessageValidator.Validate(emailMessage).IsValid);
        }

        [Theory]
        [InlineData("test1,test2")]
        [InlineData(",test2")]
        [InlineData("test1,")]
        public void MessageEmail_Cc_IsInvalid(string cc)
        {
            var emailMessage = new EmailMessage
            {
                From = "finance@aiof.com",
                To = "finance2@aiof.com",
                Subject = "This is a subject",
                Cc = cc
            };

            Assert.False(_emailMessageValidator.Validate(emailMessage).IsValid);
        }

        [Theory]
        [InlineData("test1,test2")]
        [InlineData(",test2")]
        [InlineData("test1,")]
        public void MessageEmail_Bcc_IsInvalid(string bcc)
        {
            var emailMessage = new EmailMessage
            {
                From = "finance@aiof.com",
                To = "finance2@aiof.com",
                Subject = "This is a subject",
                Bcc = bcc
            };

            Assert.False(_emailMessageValidator.Validate(emailMessage).IsValid);
        }
    }
}
