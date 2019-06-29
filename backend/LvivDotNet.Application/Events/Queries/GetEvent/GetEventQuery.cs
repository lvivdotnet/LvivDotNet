using LvivDotNet.Application.Events.Models;
using MediatR;

namespace LvivDotNet.Application.Events.Queries.GetEvent
{
    /// <summary>
    /// Get event query.
    /// </summary>
    public class GetEventQuery : IRequest<EventModel>
    {
        /// <summary>
        /// Gets or sets event id.
        /// </summary>
        public int EventId { get; set; }
    }
}
