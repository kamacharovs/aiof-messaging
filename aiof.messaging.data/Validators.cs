using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Net.Mail;

using FluentValidation;

namespace aiof.messaging.data
{
    public static class CommonValidator
    {
        public static string RegexEmail = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
        public static string RegexPhoneNumber = @" ^ (\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$";
    }

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
                })
                .WithMessage($"{nameof(Message.Type)} must be one of the following {MessageType.AllAsString}");
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
                .When(x => !string.IsNullOrWhiteSpace(x.Bcc));

            RuleFor(x => x.Body)
                .Length(1, 5000)
                .When(x => !string.IsNullOrWhiteSpace(x.Body));
        }

        public bool AreEmailsValid(string emailAddresses)
        {
            var emailAddressesSplit = emailAddresses.Split(',');

            foreach (var emailAddress in emailAddressesSplit)
            {
                var match = Regex.Match(emailAddress, CommonValidator.RegexEmail, RegexOptions.IgnoreCase);

                if (!match.Success)
                    return false;
            }

            return true;
        }
    }
}
