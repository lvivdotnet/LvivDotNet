using Lviv_.NET_Platform.Application.Events.Models;
using MediatR;

namespace Lviv_.NET_Platform.Application.Events.Queries.GetEvent
{
    public class GetEventQuery : IRequest<EventModel>
    {
        public int EventId { get; set; }
    }
}
