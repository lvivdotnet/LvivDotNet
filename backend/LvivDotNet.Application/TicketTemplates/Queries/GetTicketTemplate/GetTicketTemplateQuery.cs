using LvivDotNet.Application.TicketTemplates.Models;
using MediatR;

namespace LvivDotNet.Application.TicketTemplates.Queries.GetTicketTemplate
{
    /// <summary>
    ///  Get ticket template query.
    /// </summary>
    public class GetTicketTemplateQuery : IRequest<TicketTemplateModel>
    {
        /// <summary>
        /// Gets or sets ticket template id.
        /// </summary>
        public int Id { get; set; }
    }
}
