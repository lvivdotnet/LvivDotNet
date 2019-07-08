using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.TicketTemplates.Models;
using MediatR;

namespace LvivDotNet.Application.TicketTemplates.Commands.UpdateTicketTemplate
{
    /// <summary>
    /// Update ticket template command handler.
    /// </summary>
    public class UpdateTicketTemplateCommandHandler : BaseHandler<UpdateTicketTemplateCommand>
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
        protected override async Task<Unit> Handle(UpdateTicketTemplateCommand request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            await connection.ExecuteAsync(
                    "update dbo.[ticket_template] " +
                    "set Name = @Name, " +
                    "[From] = @From, " +
                    "[To] = @To, " +
                    "Price = @Price " +
                    "where Id = @Id",
                    request,
                    transaction);

            return Unit.Value;
        }
    }
}