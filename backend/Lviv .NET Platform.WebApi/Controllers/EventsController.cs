using Lviv_.NET_Platform.Application.Events.Commands.AddEvent;
using Lviv_.NET_Platform.Application.Events.Models;
using Lviv_.NET_Platform.Application.Events.Queries.GetEvent;
using Lviv_.NET_Platform.Application.Events.Queries.GetEvents;
using Lviv_.NET_Platform.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lviv_.NET_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : Controller
    {
        private readonly IMediator mediator;

        public EventsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public Task<int> AddEvent([FromBody] AddEventCommand command)
            => this.mediator.Send(command);

        [HttpGet("{id:int}")]
        public Task<EventModel> GetEvent(int id)
            => this.mediator.Send(new GetEventQuery { EventId = id });

        [HttpGet]
        public Task<Page<EventShortModel>> GetEvents([FromQuery] int take, [FromQuery] int skip)
            => this.mediator.Send(new GetEventsQuery { Take = take, Skip = skip });
    }
}
