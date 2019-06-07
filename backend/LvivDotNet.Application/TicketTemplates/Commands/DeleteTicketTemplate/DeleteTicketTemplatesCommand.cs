using MediatR;

namespace LvivDotNet.Application.TicketTemplates.Commands.DeleteTicketTemplate
{
    public class DeleteTicketTemplateCommand : IRequest
    {
        public int Id { get; set; }
    }
}
