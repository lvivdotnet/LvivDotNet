using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Common;

namespace LvivDotNet.Application.TicketTemplates.Commands.AddTicketTemplate
{
    public class AddTicketTemplateCommandHandler : BaseHandler<AddTicketTemplateCommand, int>
    {
        public AddTicketTemplateCommandHandler(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

        protected override async Task<int> Handle(AddTicketTemplateCommand request, CancellationToken cancellationToken, IDbConnection connection, IDbTransaction transaction)
        {
            await connection.ExecuteAsync("insert into dbo.[ticket_template](Name, EventId, Price, [From], [To])" +
                                          "values (@Name, @EventId, @Price, @From, @to)", request, transaction);

            return await DatabaseHelpers.GetLastIdentity(connection, transaction);
        }
    }
}
