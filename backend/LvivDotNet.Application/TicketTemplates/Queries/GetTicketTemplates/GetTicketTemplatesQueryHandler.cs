using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.TicketTemplates.Models;

namespace LvivDotNet.Application.TicketTemplates.Queries.GetTicketTemplates
{
    public class GetTicketTemplatesQueryHandler : BaseHandler<GetTicketTemplatesQuery, IEnumerable<TicketTemplateModel>>
    {
        public GetTicketTemplatesQueryHandler(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

        protected override async Task<IEnumerable<TicketTemplateModel>> Handle(GetTicketTemplatesQuery request, CancellationToken cancellationToken, IDbConnection connection, IDbTransaction transaction)
        {
            return await connection.QueryAsync<TicketTemplateModel>("select * from dbo.[ticket_template] where EventId = @EventId", request, transaction);
        }
    }
}
