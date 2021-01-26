using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using AutoMapper;

using aiof.messaging.data;

namespace aiof.messaging.tests
{
    [Trait(Helper.Category, Helper.UnitTest)]
    public class MappingTests
    {
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _mapper = Helper.GetRequiredService<IMapper>() ?? throw new ArgumentNullException(nameof(IMapper));
        }

        [Theory]
        [InlineData("from@aiof.com", "to@aiof.com", "subject 1")]
        [InlineData("from@aiof.com", "to@aiof.com", "subject 2")]
        public void Map_IMessage_IEmailMessage_IsSuccessful(
            string from,
            string to,
            string subject)
        {
            var message = new Message
            {
                From = from,
                To = to,
                Subject = subject
            } as IMessage;

            var emailMessage = _mapper.Map<IEmailMessage>(message);

            Assert.NotNull(emailMessage);
            Assert.Equal(from, emailMessage.From);
            Assert.Equal(to, emailMessage.To);
            Assert.Equal(subject, emailMessage.Subject);
        }

        [Theory]
        [InlineData("Email")]
        [InlineData("SMS")]
        public void Map_IMessage_MessageEntity_IsSuccessful(string type)
        {
            var message = new Message
            {
                Type = type
            } as IMessage;

            var messageEntity = _mapper.Map<MessageEntity>(message);

            Assert.NotNull(messageEntity);
            Assert.Equal(type, messageEntity.Type);
            Assert.Equal(message.PublicKey, messageEntity.PublicKey);
        }

        [Theory]
        [InlineData("from@aiof.com", "to@aiof.com", "subject 1")]
        [InlineData("from@aiof.com", "to@aiof.com", "subject 2")]
        public void Map_IEmailMessage_EmailMessageEntity_IsSuccessful(
            string from,
            string to,
            string subject)
        {
            var emailMessage = new EmailMessage
            {
                From = from,
                To = to,
                Subject = subject
            } as IEmailMessage;

            var emailMessageEntity = _mapper.Map<EmailMessageEntity>(emailMessage);

            Assert.NotNull(emailMessageEntity);
            Assert.Equal(from, emailMessageEntity.From);
            Assert.Equal(to, emailMessageEntity.To);
            Assert.Equal(subject, emailMessageEntity.Subject);
        }
    }
}
