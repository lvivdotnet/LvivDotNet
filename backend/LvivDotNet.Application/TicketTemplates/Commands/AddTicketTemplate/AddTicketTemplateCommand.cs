using LvivDotNet.Application.TicketTemplates.Models;
using MediatR;
using System;

namespace LvivDotNet.Application.TicketTemplates.Commands.AddTicketTemplate
{
    public class AddTicketTemplateCommand : IRequest<int>
    {
        public string Name { get; set; }

        public int EventId { get; set; }

        public decimal Price { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
