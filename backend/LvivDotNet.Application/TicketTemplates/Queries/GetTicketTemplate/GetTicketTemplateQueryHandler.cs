using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.TicketTemplates.Models;
using Microsoft.AspNetCore.Http;

namespace LvivDotNet.Application.TicketTemplates.Queries.GetTicketTemplate
{
    /// <summary>
    /// Get ticket template query handler.
    /// </summary>
    public class GetTicketTemplateQueryHandler : BaseHandler<GetTicketTemplateQuery, TicketTemplateModel>
    {
        /// <summary>
        /// Get ticket template sql command.
        /// </summary>
        private const string GetTicketTemplateSqlCommand =
            @"select * from public.ticket_template where ""Id"" = @Id";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTicketTemplateQueryHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        public GetTicketTemplateQueryHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        /// <inheritdoc/>
        protected override async Task<TicketTemplateModel> Handle(GetTicketTemplateQuery request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            return await connection.QuerySingleAsync<TicketTemplateModel>(GetTicketTemplateSqlCommand, request, transaction)
                .ConfigureAwait(true);
        }
    }
}