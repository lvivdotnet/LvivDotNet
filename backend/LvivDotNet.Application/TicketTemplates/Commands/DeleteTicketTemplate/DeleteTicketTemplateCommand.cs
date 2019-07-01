using MediatR;

namespace LvivDotNet.Application.TicketTemplates.Commands.DeleteTicketTemplate
{
    /// <summary>
    /// Delete ticket template command.
    /// </summary>
    public class DeleteTicketTemplateCommand : IRequest
    {
        /// <summary>
        /// Gets or sets ticket template id.
        /// </summary>
        public int Id { get; set; }
    }
}
