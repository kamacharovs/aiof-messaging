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
        private readonly ServiceBusClient _client;
        private readonly AbstractValidator<IMessage> _messageValidator;
        private readonly AbstractValidator<IEmailMessage> _emailMessageValidator;

        public MessageRepository(
            ILogger<MessageRepository> logger,
            IEnvConfiguration envConfig,
            IMapper mapper,
            ServiceBusClient client,
            AbstractValidator<IMessage> messageValidator,
            AbstractValidator<IEmailMessage> emailMessageValidator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _envConfig = envConfig ?? throw new ArgumentNullException(nameof(envConfig));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _messageValidator = messageValidator ?? throw new ArgumentNullException(nameof(messageValidator));
            _emailMessageValidator = emailMessageValidator ?? throw new ArgumentNullException(nameof(emailMessageValidator));
        }

        public async Task SendInboundMessageAsync(IMessage message)
        {
            await SendMessageAsync(_envConfig.InboundQueueName, message);
        }

        public async Task SendEmailMessageAsync(IEmailMessage message)
        {
            await SendMessageAsync(_envConfig.EmailQueueName, message);
        }

        public async Task SendMessageAsync(
            string queue,
            object message)
        {
            var msgStr = JsonConvert.SerializeObject(message);

            try
            {
                var sender = _client.CreateSender(queue);
                var sbMessage = new ServiceBusMessage(msgStr);

                await sender.SendMessageAsync(sbMessage);

                _logger.LogInformation("Sent message={message} to queue={queue}",
                    msgStr,
                    queue);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while sending {queue} message", queue);
            }
        }
        public async Task SendMessagesAsync(
            string queue,
            IEnumerable<object> messages)
        {
            var msgsStr = JsonConvert.SerializeObject(messages);

            try
            {
                var sender = _client.CreateSender(queue);
                var sbMessages = new ServiceBusMessage(msgsStr);

                await sender.SendMessageAsync(sbMessages);

                _logger.LogInformation("Sent message count={messageCount} to queue={queue}",
                    messages.Count(),
                    queue);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while sending {queue} message", queue);
            }
        }

        public async Task SendAsync(IMessage message)
        {
            if (message.Type == MessageType.Email)
            {
                var emailMsg = _mapper.Map<IEmailMessage>(message);

                try
                {
                    await _emailMessageValidator.ValidateAndThrowAsync(emailMsg);
                }
                catch (ValidationException e)
                {
                    _logger.LogError(e,
                        "Validation error while processing {queue} with message={message}",
                        _envConfig.InboundQueueName,
                        message);

                    //TODO just dead letter queue on validation error
                    return;
                }

                await SendEmailMessageAsync(emailMsg);
            }
        }
    }
}
