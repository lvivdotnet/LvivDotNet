using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.TicketTemplates.Models;
using Microsoft.AspNetCore.Http;

namespace LvivDotNet.Application.TicketTemplates.Queries.GetTicketTemplates
{
    /// <summary>
    /// Get ticket templates query handler.
    /// </summary>
    public class GetTicketTemplatesQueryHandler : BaseHandler<GetTicketTemplatesQuery, IEnumerable<TicketTemplateModel>>
    {
        /// <summary>
        /// Get ticket templates sql query.
        /// </summary>
        private const string GetTicketTemplatesSqlQuery = @"select * from public.ticket_template where ""EventId"" = @EventId";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTicketTemplatesQueryHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        public GetTicketTemplatesQueryHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        /// <inheritdoc/>
        protected override async Task<IEnumerable<TicketTemplateModel>> Handle(GetTicketTemplatesQuery request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            return await connection.QueryAsync<TicketTemplateModel>(GetTicketTemplatesSqlQuery, request, transaction).ConfigureAwait(false);
        }
    }
}
