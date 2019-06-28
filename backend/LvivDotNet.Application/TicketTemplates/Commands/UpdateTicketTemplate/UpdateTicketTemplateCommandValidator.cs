using FluentValidation;

namespace LvivDotNet.Application.TicketTemplates.Commands.UpdateTicketTemplate
{
    public class UpdateTicketTemplateCommandValidator : AbstractValidator<UpdateTicketTemplateCommand>
    {
        public UpdateTicketTemplateCommandValidator()
        {
            RuleFor(c => c.Id).GreaterThan(0);
            RuleFor(c => c.To).NotEmpty().GreaterThan(c => c.From);
            RuleFor(c => c.From).NotEmpty().LessThan(c => c.To);
            RuleFor(c => c.Price).GreaterThanOrEqualTo(0);
        }
    }
}