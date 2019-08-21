using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Exceptions;
using LvivDotNet.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

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
        /// <param name="httpContextAccessor"> See <see cref="IHttpContextAccessor"/>. </param>
        public DeleteTicketTemplateCommandHandler(IDbConnectionFactory dbConnectionFactory, IHttpContextAccessor httpContextAccessor)
            : base(dbConnectionFactory, httpContextAccessor)
        {
        }

        /// <inheritdoc/>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "We already have a not-null check for request in MediatR")]
        protected override async Task<Unit> Handle(DeleteTicketTemplateCommand request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
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
