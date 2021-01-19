using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
                .Length(3, 100);

            RuleFor(x => x.To)
                .NotNull()
                .NotEmpty()
                .Length(3, 100);

            RuleFor(x => x.Subject)
                .NotNull()
                .NotEmpty()
                .Length(1, 300);
        }
    }
}
