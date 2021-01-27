using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using Moq;

using aiof.messaging.data;
using aiof.messaging.services;

namespace aiof.messaging.tests
{
    [Trait(Helper.Category, Helper.UnitTest)]
    public class TableRepositoryTests
    {
        private readonly ITableRepository _repo;
        private readonly Mock<ITableRepository> _mockRepo;

        public TableRepositoryTests()
        {
            _mockRepo = Helper.GetMockTableRepository() ?? throw new ArgumentNullException(nameof(Mock<ITableRepository>));
            _repo = _mockRepo.Object ?? throw new ArgumentNullException(nameof(ITableRepository));
        }

        [Theory]
        [InlineData(MessageType.Email)]
        [InlineData(MessageType.Sms)]
        public async Task LogAsync_IMessage_IsSuccessful(string type)
        {
            var message = new Message
            {
                Type = type
            } as IMessage;

            await _repo.LogAsync(message);

            _mockRepo.Verify(x => x.LogAsync(message), Times.Once());
        }

        [Theory]
        [InlineData("from1@aiof.com", "to1@aiof.com")]
        [InlineData("from2@aiof.com", "to2@aiof.com")]
        [InlineData("from3@aiof.com", "to3@aiof.com")]
        public async Task LogAsync_IEmailMessage_IsSuccessful(
            string from,
            string to)
        {
            var message = new EmailMessage
            {
                From = from,
                To = to
            } as IMessage;

            await _repo.LogAsync(message);

            _mockRepo.Verify(x => x.LogAsync(message), Times.Once());
        }

        [Theory]
        [InlineData("queue1", MessageType.Email)]
        [InlineData("queue2", MessageType.Sms)]
        public async Task InsertAsync_MessageEntity_IsSuccessful(
            string queueName,
            string type)
        {
            var message = new MessageEntity
            {
                Type = type
            };

            await _repo.InsertAsync(queueName, message);

            _mockRepo.Verify(x => x.InsertAsync(queueName, message), Times.Once());
        }
    }
}
