using System.Collections.Generic;
using System.Threading.Tasks;
using Lviv_.NET_Platform.Application.Events.Commands.AddEvent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lviv_.NET_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : Controller
    {
        private readonly IMediator mediator;

        public EventsController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddEvent([FromBody] AddEventCommand command)
        {
            return Ok(await this.mediator.Send(command));
        }
    }
}
