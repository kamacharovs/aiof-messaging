using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using Xunit;

using aiof.messaging.data;

namespace aiof.messaging.tests
{
    [Trait(Helper.Category, Helper.UnitTest)]
    public class ValidatorTests
    {
        private readonly AbstractValidator<IMessage> _messageValidator;

        public ValidatorTests()
        {
            _messageValidator = Helper.GetRequiredService<AbstractValidator<IMessage>>() ?? throw new ArgumentNullException(nameof(AbstractValidator<IMessage>));;
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
        public void Message_Validate_Type_DoesntExist(string type)
        {
            var message = new Message
            {
                Type = type
            };

            Assert.False(_messageValidator.Validate(message).IsValid);
        }
    }
}
