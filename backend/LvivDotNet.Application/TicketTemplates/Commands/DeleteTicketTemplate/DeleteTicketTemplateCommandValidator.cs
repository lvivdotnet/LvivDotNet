using FluentValidation;

namespace LvivDotNet.Application.TicketTemplates.Commands.DeleteTicketTemplate
{
    /// <summary>
    /// Delete ticket template command validation rules.
    /// </summary>
    public class DeleteTicketTemplateCommandValidator : AbstractValidator<DeleteTicketTemplateCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteTicketTemplateCommandValidator"/> class.
        /// </summary>
        public DeleteTicketTemplateCommandValidator()
        {
            this.RuleFor(c => c.Id).NotEmpty();
        }
    }
}
