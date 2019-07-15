using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LvivDotNet.Application.Tickets.Commands.BuyTicket.Authorized;
using LvivDotNet.Application.Tickets.Models;
using LvivDotNet.Application.Tickets.Queries.GetTicket;
using LvivDotNet.Application.Tickets.Queries.GetUserTickets;
using LvivDotNet.Common.Extensions;
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
    [Authorize]
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
        /// Buy ticket by authorized user.
        /// </summary>
        /// <param name="command"> Buy ticket command: <see cref="BuyTicketCommand"/>. </param>
        /// <returns> Task representing asynchronous operation. </returns>
        [HttpPost]
        public Task<int> BuyTicket(BuyTicketCommand command)
            => this.Mediator.Send(command);

        /// <summary>
        /// Get all user tickets by user id.
        /// </summary>
        /// <returns> Collection of <see cref="TicketModel"/> bought by user. </returns>
        [HttpGet]
        public Task<IEnumerable<TicketModel>> GetUserTickets()
            => this.Mediator.Send(new GetUserTicketsQuery { UserId = this.User.GetId() });

        /// <summary>
        /// Get ticket by ticket id.
        /// </summary>
        /// <param name="id"> Ticket id. </param>
        /// <returns> <see cref="TicketModel"/>. </returns>
        [HttpGet("{id:int}")]
        public Task<TicketModel> GetTicketById(int id)
            => this.Mediator.Send(new GetTicketQuery { TicketId = id, UserId = this.User.GetId() });
    }
}
