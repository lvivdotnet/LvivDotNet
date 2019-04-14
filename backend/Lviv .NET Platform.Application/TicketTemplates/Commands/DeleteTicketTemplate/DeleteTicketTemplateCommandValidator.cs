using FluentValidation;

namespace Lviv_.NET_Platform.Application.TicketTemplates.Commands.DeleteTicketTemplate
{
    public class DeleteTicketTemplateCommandValidator : AbstractValidator<DeleteTicketTemplateCommand>
    {
        public DeleteTicketTemplateCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}
