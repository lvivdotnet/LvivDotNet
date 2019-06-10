using FluentValidation;

namespace LvivDotNet.Application.TicketTemplates.Commands.AddTicketTemplate
{
    public class AddTicketTemplateCommandValidator : AbstractValidator<AddTicketTemplateCommand>
    {
        public AddTicketTemplateCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.EventId).NotEmpty().NotEqual(0);
            RuleFor(c => c.Price).NotEmpty().NotEqual(0);
            RuleFor(c => c.From).NotEmpty().LessThan(c => c.To);
            RuleFor(c => c.To).NotEmpty().LessThan(c => c.From);
        }
    }
}
