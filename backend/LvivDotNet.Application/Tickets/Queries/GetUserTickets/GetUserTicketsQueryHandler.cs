﻿using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.Tickets.Models;

namespace LvivDotNet.Application.Tickets.Queries.GetUserTickets
{
    /// <summary>
    /// Get all user ticket query handler.
    /// </summary>
    public class GetUserTicketsQueryHandler : BaseHandler<GetUserTicketsQuery, IEnumerable<TicketModel>>
    {
        /// <summary>
        /// Get user tickets sql query.
        /// </summary>
        private const string GetUserTicketsSqlQuery =
                "select " +
                @"""event"".""Name"" as EventName," +
                @"""event"".""StartDate"" as Start," +
                @"""event"".""EndDate"" as End," +
                @"""ticket"".""CreatedDate"" as Bought," +
                @"""ticket_template"".""Price"" as Price " +
                "from public.ticket as ticket " +
                @"join public.ticket_template as ticket_template on ""ticket_template"".""Id"" = ""ticket"".""TicketTemplateId"" " +
                @"join public.event as event on ""event"".""Id"" = ""ticket_template"".""EventId"" " +
                @"where ""ticket"".""UserId"" = cast(@UserId as integer)";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserTicketsQueryHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        public GetUserTicketsQueryHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        /// <inheritdoc/>
        protected override async Task<IEnumerable<TicketModel>> Handle(GetUserTicketsQuery request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            return await connection.QueryAsync<TicketModel>(GetUserTicketsSqlQuery, request, transaction)
                .ConfigureAwait(false);
        }
    }
}
