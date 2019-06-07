using FluentValidation;

namespace LvivDotNet.Application.TicketTemplates.Commands.DeleteTicketTemplate
{
    public class DeleteTicketTemplateCommandValidator : AbstractValidator<DeleteTicketTemplateCommand>
    {
        public DeleteTicketTemplateCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}
