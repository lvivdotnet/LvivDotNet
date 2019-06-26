using LvivDotNet.Application.Events.Models;
using LvivDotNet.Common;
using MediatR;

namespace LvivDotNet.Application.Events.Queries.GetEvents
{
    /// <summary>
    /// Get events query.
    /// </summary>
    public class GetEventsQuery : IRequest<Page<EventShortModel>>
    {
        /// <summary>
        /// Gets or sets skip value.
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Gets or sets take value.
        /// </summary>
        public int Take { get; set; }
    }
}