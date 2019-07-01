using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LvivDotNet.Application.Tickets.Commands.BuyTicket.Authorized;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LvivDotNet.WebApi.Controllers
{
    /// <summary>
    /// Tickets controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TicketsController"/> class.
        /// </summary>
        /// <param name="mediator"> Mediator service. </param>
        public TicketsController(IMediator mediator)
        {
            this.Mediator = mediator;
        }

        /// <summary>
        /// Gets mediator service.
        /// </summary>
        public IMediator Mediator { get; }

        /// <summary>
        /// But ticket by authorized user.
        /// </summary>
        /// <param name="command"> Buy ticket command: <see cref="BuyTicketCommand"/>. </param>
        /// <returns> Task representing asynchronous operation. </returns>
        [HttpPost]
        public Task BuyTicket(BuyTicketCommand command)
            => this.Mediator.Send(command);
    }
}
