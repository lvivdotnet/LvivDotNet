using Lviv_.NET_Platform.Application.Events.Models;
using Lviv_.NET_Platform.Common;
using MediatR;

namespace Lviv_.NET_Platform.Application.Events.Queries.GetEvents
{
    public class GetEventsQuery: IRequest<Page<EventShortModel>>
    {
        public int Skip { get; set; }

        public int Take { get; set; }
    }
}