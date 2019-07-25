using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Exceptions;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Common;

namespace LvivDotNet.Application.Tickets.Commands.BuyTicket.Unauthorized
{
    /// <summary>
    /// Buy ticket by unauthorized user command handler.
    /// </summary>
    public class BuyUnauthorizedTicketCommandHandler : BaseHandler<BuyUnauthorizedTicketCommand, int>
    {
        /// <summary>
        /// Get ticket info sql query.
        /// </summary>
        private const string GetTicketInfoSqlQuery =
                "select " +
                "count(*) as ticketsCount, " +
                @"(select ""MaxAttendees"" from public.event where ""Id"" = cast(@EventId as integer)) as maxAttendees, " +
                @"(select ""Id"" from public.ticket_template where ""EventId"" = cast(@EventId as integer) and ""From"" <= @Now and ""To"" >= @Now limit 1) as ticketTemplateId " +
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
        /// Insert attendee sql command.
        /// </summary>
        private const string InsertAttendeeSqlCommand =
                @"insert into public.attendee (""FirstName"", ""LastName"", ""Email"", ""Phone"", ""Male"", ""Age"") " + // Insert new attendee.
                "values (@FirstName, @LastName, @Email, @Phone, @Male, @Age) " +
                @"returning ""Id"";";

        /// <summary>
        /// Insert ticket sql command.
        /// </summary>
        private const string InsertTicketSqlCommand =
                @"insert into public.ticket (""TicketTemplateId"", ""AttendeeId"", ""UserId"", ""CreatedDate"") " + // Insert new ticket.
                "values (@TicketTemplateId, @AttendeeId, NULL, @CreatedDate) " +
                @"returning ""Id"";";

        /// <summary>
        /// Initializes a new instance of the <see cref="BuyUnauthorizedTicketCommandHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        public BuyUnauthorizedTicketCommandHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        /// <inheritdoc/>
        protected override async Task<int> Handle(BuyUnauthorizedTicketCommand request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            (var ticketsCount, var maxAttendees, var ticketTemplateId) =
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

                throw new SouldOutException(eventName);
            }

            var ateendeeId = await connection.QuerySingleAsync<int>(InsertAttendeeSqlCommand, request, transaction);

            var insertTicketParams = new { TicketTemplateId = ticketTemplateId, CreatedDate = DateTime.UtcNow, AttendeeId = ateendeeId, };
            return await connection.QuerySingleAsync<int>(InsertTicketSqlCommand, insertTicketParams, transaction);
        }
    }
}
