using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.TicketTemplates.Models;

namespace LvivDotNet.Application.TicketTemplates.Queries.GetTicketTemplate
{
    public class GetTicketTemplateQueryHandler : BaseHandler<GetTicketTemplateQuery, TicketTemplateModel>
    {
        public GetTicketTemplateQueryHandler(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

        protected override Task<TicketTemplateModel> Handle(GetTicketTemplateQuery request, CancellationToken cancellationToken, IDbConnection connection, IDbTransaction transaction)
        {
            return connection.QuerySingleAsync<TicketTemplateModel>(
                    "select * from dbo.[ticket_template] where Id = @Id",
                    request, transaction
                );
        }
    }
}