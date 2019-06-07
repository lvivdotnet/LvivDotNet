using LvivDotNet.Application.Events.Models;
using MediatR;

namespace LvivDotNet.Application.Events.Queries.GetEvent
{
    public class GetEventQuery : IRequest<EventModel>
    {
        public int EventId { get; set; }
    }
}
