using FluentValidation;
using TestTask.Domain.Models;

namespace TestTask.Clients.Validators
{
    public class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(x => x.Text)
                .MaximumLength(128).WithMessage("Message must be 128 length max.");
        }
    }
}
