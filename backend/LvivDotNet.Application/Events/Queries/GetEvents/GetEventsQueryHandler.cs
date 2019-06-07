using LvivDotNet.Application.Events.Models;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Common;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace LvivDotNet.Application.Events.Queries.GetEvents
{
    public class GetEventsQueryHandler : BaseHandler<GetEventsQuery, Page<EventShortModel>>
    {
        public GetEventsQueryHandler(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

        protected override Task<Page<EventShortModel>> Handle(GetEventsQuery request, CancellationToken cancellationToken, IDbConnection connection, IDbTransaction transaction)
        {
            throw new System.NotImplementedException();
        }
    }
}