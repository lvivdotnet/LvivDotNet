using LvivDotNet.Application.TicketTemplates.Models;
using MediatR;

namespace LvivDotNet.Application.TicketTemplates.Queries.GetTicketTemplate
{
    public class GetTicketTemplateQuery : IRequest<TicketTemplateModel>
    {
        public int Id { get; set; }
    }
}
