using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
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
            await _messageValidator.ValidateAndThrowAsync(message);
            await SendMessageAsync(_envConfig.InboundQueueName, message);
        }

        public async Task SendEmailMessageAsync(IEmailMessage message)
        {
            await _emailMessageValidator.ValidateAndThrowAsync(message);
            await SendMessageAsync(_envConfig.EmailQueueName, message);
        }

        public async Task SendMessageAsync(
            string queue,
            object message)
        {
            var msgStr = JsonConvert.SerializeObject(message);

            var sender = _client.CreateSender(queue);
            var sbMessage = new ServiceBusMessage(msgStr);

            await sender.SendMessageAsync(sbMessage);

            _logger.LogInformation("Sent message={message} to queue={queue}",
                msgStr,
                queue);
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

                var emailMsg = _mapper.Map<IEmailMessage>(message);

                /*
                 * Log EmailMessage to table
                 */
                await _tableRepo.LogAsync(message, emailMsg);

                /*
                 * Send email message to email queue
                 */
                await SendEmailMessageAsync(emailMsg);
            }
        }
    }
}
