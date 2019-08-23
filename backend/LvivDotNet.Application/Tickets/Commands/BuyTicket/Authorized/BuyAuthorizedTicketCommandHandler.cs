using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Exceptions;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Common.Extensions;
using Microsoft.AspNetCore.Http;

namespace LvivDotNet.Application.Tickets.Commands.BuyTicket.Authorized
{
    /// <summary>
    /// Buy ticket command handler for authorized user.
    /// </summary>
    public class BuyAuthorizedTicketCommandHandler : BaseHandler<BuyAuthorizedTicketCommand, int>
    {
        /// <summary>
        /// Get ticket info sql query.
        /// </summary>
        private const string GetTicketInfoSqlQuery =
                "select " +
                "count(*) as ticketsCount, " +
                @"(select ""MaxAttendees"" from public.event where ""Id"" = @EventId) as maxAttendees, " +
                @"(select ""Id"" from public.ticket_template where ""EventId"" = @EventId and ""From"" <= @Now and ""To"" >= @Now limit 1) as ticketTemplateId " +
                "from public.ticket as ticket " +
                @"join public.ticket_template as ticket_template on ""ticket"".""TicketTemplateId"" = ""ticket_template"".""Id"" " +
                @"where ""ticket_template"".""EventId"" = cast(@EventId as integer)";

        /// <summary>
        /// Get event name sql query.
        /// </summary>
        private const string GetEventNameSqlQuery =
                    @"select ""Name"" from public.event as event " +
                    @"where ""event"".""Id"" = @EventId";

        /// <summary>
        /// Insert ticket sql command.
        /// </summary>
        private const string InsertTicketSqlCommand =
                @"insert into public.ticket (""TicketTemplateId"", ""AttendeeId"", ""UserId"", ""CreatedDate"") " +
                @"select @TicketTemplateId, NULL, ""Id"", @CreatedDate from public.user " +
                @"where ""Id"" = @UserId " +
                @"returning ""Id""";

        /// <summary>
        /// Initializes a new instance of the <see cref="BuyAuthorizedTicketCommandHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        /// <param name="httpContextAccessor"> See <see cref="IHttpContextAccessor"/>. </param>
        public BuyAuthorizedTicketCommandHandler(IDbConnectionFactory dbConnectionFactory, IHttpContextAccessor httpContextAccessor)
            : base(dbConnectionFactory, httpContextAccessor)
        {
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "We already have a not-null check for request in MediatR")]
        protected override async Task<int> Handle(BuyAuthorizedTicketCommand request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            var (ticketsCount, maxAttendees, ticketTemplateId) =
                await connection.QuerySingleAsync<(int ticketsCount, int maxAttendees, int? ticketTemplateId)>(
                    GetTicketInfoSqlQuery, new { request.EventId, Now = DateTime.UtcNow }, transaction)
                .ConfigureAwait(false);

            if (!ticketTemplateId.HasValue)
            {
                throw new TicketsNotAvailable();
            }

            if (ticketsCount >= maxAttendees)
            {
                var eventName = await connection.QuerySingleAsync<string>(GetEventNameSqlQuery, new { request.EventId }, transaction)
                    .ConfigureAwait(false);

                throw new SoldOutException(eventName);
            }

            return await connection.QuerySingleAsync<int>(InsertTicketSqlCommand, new { TicketTemplateId = ticketTemplateId, UserId = this.User.GetId(), CreatedDate = DateTime.UtcNow }, transaction)
                .ConfigureAwait(false);
        }
    }
}
