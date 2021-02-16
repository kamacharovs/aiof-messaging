using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

using FluentValidation;

namespace aiof.messaging.data
{
    public class MessageValidator : AbstractValidator<IMessage>
    {
        public MessageValidator()
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Type)
                .NotNull()
                .NotEmpty()
                .Must(x =>
                {
                    return MessageType.All
                        .Contains(x)
                        ? true
                        : false;
                });
        }
    }

    public class EmailMessageValidator : AbstractValidator<IEmailMessage>
    {
        public EmailMessageValidator()
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.From)
                .NotNull()
                .NotEmpty()
                .Length(3, 100)
                .EmailAddress();

            RuleFor(x => x.To)
                .NotNull()
                .NotEmpty()
                .Length(3, 100)
                .EmailAddress();

            RuleFor(x => x.Subject)
                .NotNull()
                .NotEmpty()
                .Length(1, 300);

            RuleFor(x => x.Cc)
                .Must(x =>
                {
                    return AreEmailsValid(x);
                })
                .When(x => !string.IsNullOrWhiteSpace(x.Cc));

            RuleFor(x => x.Bcc)
                .Must(x =>
                {
                    return AreEmailsValid(x);
                })
                .When(x => !string.IsNullOrWhiteSpace(x.Cc));

            RuleFor(x => x.Body)
                .Length(1, 5000)
                .When(x => !string.IsNullOrWhiteSpace(x.Body));
        }

        public bool AreEmailsValid(string emailAddresses)
        {
            try
            {
                var emailAddressesSplit = emailAddresses.Split(',');

                foreach (var emailaddress in emailAddressesSplit)
                    _ = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
