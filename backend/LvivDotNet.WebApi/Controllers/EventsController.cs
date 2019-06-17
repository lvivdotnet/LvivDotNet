using System.Collections.Generic;
using LvivDotNet.Application.Events.Commands.AddEvent;
using LvivDotNet.Application.Events.Commands.UpdateEvent;
using LvivDotNet.Application.Events.Models;
using LvivDotNet.Application.Events.Queries.GetEvent;
using LvivDotNet.Application.Events.Queries.GetEvents;
using LvivDotNet.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.TicketTemplates.Models;
using LvivDotNet.Application.TicketTemplates.Queries.GetTicketTemplates;

namespace LvivDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : Controller
    {
        public IMediator Mediator { get; }

        public EventsController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost]
        public Task<int> AddEvent([FromBody] AddEventCommand command)
            => this.Mediator.Send(command);

        [HttpGet("{id:int}")]
        public Task<EventModel> GetEvent(int id)
            => this.Mediator.Send(new GetEventQuery { EventId = id });

        [HttpGet("ticket/templates/{eventId:int}")]
        public Task<IEnumerable<TicketTemplateModel>> GetTicketTemplates(int eventId)
            => this.Mediator.Send(new GetTicketTemplatesQuery { EventId = eventId });

        [HttpGet]
        public Task<Page<EventShortModel>> GetEvents([FromQuery] int take, [FromQuery] int skip)
            => this.Mediator.Send(new GetEventsQuery { Take = take, Skip = skip });

        [HttpPut]
        public Task UpdateEvent([FromBody] UpdateEventCommand command)
            => this.Mediator.Send(command);
    }
}
