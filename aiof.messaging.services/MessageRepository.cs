using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.Extensions.Logging;
using Azure.Messaging.ServiceBus;

using Newtonsoft.Json;
using AutoMapper;
using FluentValidation;

using aiof.messaging.data;

namespace aiof.messaging.services
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ILogger<MessageRepository> _logger;
        private readonly IEnvConfiguration _envConfig;
        private readonly IMapper _mapper;
        private readonly ITestConfigRepository _testConfigRepo;
        private readonly ITableRepository _tableRepo;
        private readonly ServiceBusClient _client;
        private readonly AbstractValidator<IMessage> _messageValidator;
        private readonly AbstractValidator<IEmailMessage> _emailMessageValidator;

        public MessageRepository(
            ILogger<MessageRepository> logger,
            IEnvConfiguration envConfig,
            IMapper mapper,
            ITestConfigRepository testConfigRepo,
            ITableRepository tableRepo,
            ServiceBusClient client,
            AbstractValidator<IMessage> messageValidator,
            AbstractValidator<IEmailMessage> emailMessageValidator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _envConfig = envConfig ?? throw new ArgumentNullException(nameof(envConfig));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _testConfigRepo = testConfigRepo ?? throw new ArgumentNullException(nameof(testConfigRepo));
            _tableRepo = tableRepo ?? throw new ArgumentNullException(nameof(tableRepo));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _messageValidator = messageValidator ?? throw new ArgumentNullException(nameof(messageValidator));
            _emailMessageValidator = emailMessageValidator ?? throw new ArgumentNullException(nameof(emailMessageValidator));
        }

        public async Task SendInboundMessageAsync(IMessage message)
        {
            await _tableRepo.LogAsync(message);
            await _messageValidator.ValidateAndThrowAsync(message);
            await SendMessageAsync(message);
        }

        public async Task SendMessageAsync<T>(T message)
            where T : IMessage
        {
            var messageStr = JsonConvert.SerializeObject(message);
            var queueName = _envConfig.InboundQueueName;
            var sender = _client.CreateSender(queueName);
            var serviceBusMessage = new ServiceBusMessage(messageStr);

            await sender.SendMessageAsync(serviceBusMessage);

            _logger.LogInformation("Sent message={message} to queue={queue}",
                messageStr,
                queueName);
        }

        public async Task SendEmailMessageAsync<T>(T message)
            where T : IEmailMessage
        {
            var emailMessageStr = JsonConvert.SerializeObject(message);
            var queueName = _envConfig.EmailQueueName;
            var sender = _client.CreateSender(queueName);
            var serviceBusMessage = new ServiceBusMessage(emailMessageStr);

            await _tableRepo.LogAsync(message);

            await sender.SendMessageAsync(serviceBusMessage);

            _logger.LogInformation("Sent message={message} to queue={queue}",
                emailMessageStr,
                queueName);
        }

        public async Task SendAsync(IMessage message)
        {
            if (message.Type == MessageType.Email)
            {
                if (message.TestConfig?.IsTest == true
                    && message.TestConfig?.UseConfig == true)
                {
                    var id = (int)message.TestConfig.Id;
                    var testConfig = await _testConfigRepo.GetAsync(id);

                    message.To = testConfig.Email;
                    message.Subject = testConfig.Subject ?? $"[Message Test Email] Id={id}";
                    message.Cc = null;
                    message.Bcc = null;
                }

                var emailMessage = _mapper.Map<IEmailMessage>(message);

                /*
                 * Send email message to email queue
                 */
                await _emailMessageValidator.ValidateAndThrowAsync(emailMessage);
                await SendEmailMessageAsync(emailMessage);
            }
        }
    }
}
