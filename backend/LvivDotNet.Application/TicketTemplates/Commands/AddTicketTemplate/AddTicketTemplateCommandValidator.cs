using FluentValidation;

namespace LvivDotNet.Application.TicketTemplates.Commands.AddTicketTemplate
{
    /// <summary>
    /// Add ticket template command validation rules.
    /// </summary>
    public class AddTicketTemplateCommandValidator : AbstractValidator<AddTicketTemplateCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddTicketTemplateCommandValidator"/> class.
        /// </summary>
        public AddTicketTemplateCommandValidator()
        {
            this.RuleFor(c => c.Name).NotEmpty();
            this.RuleFor(c => c.EventId).NotEmpty().NotEqual(0);
            this.RuleFor(c => c.Price).NotEmpty().NotEqual(0);
            this.RuleFor(c => c.From).NotEmpty().LessThan(c => c.To);
            this.RuleFor(c => c.To).NotEmpty().LessThan(c => c.From);
        }
    }
}
