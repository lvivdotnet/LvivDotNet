using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Exceptions;
using LvivDotNet.Application.Interfaces;
using MediatR;

namespace LvivDotNet.Application.TicketTemplates.Commands.DeleteTicketTemplate
{
    /// <summary>
    /// Delete ticket template command handler.
    /// </summary>
    public class DeleteTicketTemplateCommandHandler : BaseHandler<DeleteTicketTemplateCommand>
    {
        /// <summary>
        /// Delete ticket template sql command.
        /// </summary>
        private const string DeleteTicketTemplateSqlCommand =
                    "delete from public.ticket_template " +
                    @"where ""Id"" = @Id";

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteTicketTemplateCommandHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        public DeleteTicketTemplateCommandHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        /// <inheritdoc/>
        protected override async Task<Unit> Handle(DeleteTicketTemplateCommand request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var deletedCount = await connection.ExecuteAsync(DeleteTicketTemplateSqlCommand, request, transaction)
                .ConfigureAwait(true);

            if (deletedCount == 0)
            {
                throw new NotFoundException("Ticket Template", request.Id);
            }

            return Unit.Value;
        }
    }
}
