using System.Data;
using System.Threading;
using System.Threading.Tasks;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.TicketTemplates.Models;

namespace LvivDotNet.Application.TicketTemplates.Commands.UpdateTicketTemplate
{
    /// <summary>
    /// Update ticket template command handler.
    /// </summary>
    public class UpdateTicketTemplateCommandHandler : BaseHandler<UpdateTicketTemplateCommand, TicketTemplateModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTicketTemplateCommandHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        public UpdateTicketTemplateCommandHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        /// <inheritdoc />
        protected override Task<TicketTemplateModel> Handle(UpdateTicketTemplateCommand request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}