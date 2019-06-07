using LvivDotNet.Application.Events.Models;
using LvivDotNet.Common;
using MediatR;

namespace LvivDotNet.Application.Events.Queries.GetEvents
{
    public class GetEventsQuery : IRequest<Page<EventShortModel>>
    {
        public int Skip { get; set; }

        public int Take { get; set; }
    }
}