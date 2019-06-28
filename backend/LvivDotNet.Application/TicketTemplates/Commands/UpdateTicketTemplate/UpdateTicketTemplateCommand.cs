using System;
using LvivDotNet.Application.TicketTemplates.Models;
using MediatR;

namespace LvivDotNet.Application.TicketTemplates.Commands.UpdateTicketTemplate
{
    public class UpdateTicketTemplateCommand : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
