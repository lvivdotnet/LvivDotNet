using MediatR;

namespace Lviv_.NET_Platform.Application.TicketTemplates.Commands.DeleteTicketTemplate
{
    public class DeleteTicketTemplateCommand : IRequest
    {
        public int Id { get; set; }
    }
}
