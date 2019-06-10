using System.Collections.Generic;
using LvivDotNet.Application.TicketTemplates.Models;
using MediatR;

namespace LvivDotNet.Application.TicketTemplates.Queries.GetTicketTemplates
{
    public class GetTicketTemplatesQuery : IRequest<IEnumerable<TicketTemplateModel>>
    {
        public int EventId { get; set; }
    }
}
