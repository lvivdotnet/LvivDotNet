using System.Collections.Generic;
using LvivDotNet.Application.TicketTemplates.Models;
using MediatR;

namespace LvivDotNet.Application.TicketTemplates.Queries.GetTicketTemplates
{
    /// <summary>
    /// Get ticket templates query.
    /// </summary>
    public class GetTicketTemplatesQuery : IRequest<IEnumerable<TicketTemplateModel>>
    {
        /// <summary>
        /// Gets or sets event id.
        /// </summary>
        public int EventId { get; set; }
    }
}
