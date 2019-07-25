using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Common;

namespace LvivDotNet.Application.TicketTemplates.Commands.AddTicketTemplate
{
    /// <summary>
    /// Add ticket template command handler.
    /// </summary>
    public class AddTicketTemplateCommandHandler : BaseHandler<AddTicketTemplateCommand, int>
    {
        /// <summary>
        /// Add ticket template sql command.
        /// </summary>
        private const string AddTicketTemplateSqlCommand =
                @"insert into public.ticket_template(""Name"", ""EventId"", ""Price"", ""From"", ""To"")" +
                "values (@Name, @EventId, @Price, @From, @To) " +
                @"returning ""Id""";

        /// <summary>
        /// Initializes a new instance of the <see cref="AddTicketTemplateCommandHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        public AddTicketTemplateCommandHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        /// <inheritdoc/>
        protected override async Task<int> Handle(AddTicketTemplateCommand request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            return await connection.QuerySingleAsync<int>(AddTicketTemplateSqlCommand, request, transaction)
                .ConfigureAwait(true);
        }
    }
}
