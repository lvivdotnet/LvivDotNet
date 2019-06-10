using System.Collections;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LvivDotNet.Application.TicketTemplates.Commands.AddTicketTemplate;
using LvivDotNet.Application.TicketTemplates.Commands.DeleteTicketTemplate;
using LvivDotNet.Application.TicketTemplates.Models;
using LvivDotNet.Application.TicketTemplates.Queries.GetTicketTemplate;
using LvivDotNet.Application.TicketTemplates.Queries.GetTicketTemplates;

namespace LvivDotNet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketTemplatesController : ControllerBase
    {
        private readonly IMediator mediator;

        public TicketTemplatesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public Task<int> AddTicketTemplate(AddTicketTemplateCommand command)
            => this.mediator.Send(command);

        [HttpGet("{id:int}")]
        public Task<TicketTemplateModel> GetTicketTemplate(int id)
            => this.mediator.Send(new GetTicketTemplateQuery { Id = id });

        [HttpDelete("{id:int}")]
        public Task DeleteTicketTemplate(int id)
            => this.mediator.Send(new DeleteTicketTemplateCommand {Id = id});
    }
}