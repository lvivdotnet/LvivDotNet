using FluentValidation;

namespace LvivDotNet.Application.TicketTemplates.Commands.UpdateTicketTemplate
{
    /// <summary>
    /// Update ticket template command validation rules.
    /// </summary>
    public class UpdateTicketTemplateCommandValidator : AbstractValidator<UpdateTicketTemplateCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTicketTemplateCommandValidator"/> class.
        /// </summary>
        public UpdateTicketTemplateCommandValidator()
        {
            this.RuleFor(c => c.Id).GreaterThan(0);
            this.RuleFor(c => c.To).NotEmpty().GreaterThan(c => c.From);
            this.RuleFor(c => c.From).NotEmpty().LessThan(c => c.To);
            this.RuleFor(c => c.Price).GreaterThanOrEqualTo(0);
        }
    }
}