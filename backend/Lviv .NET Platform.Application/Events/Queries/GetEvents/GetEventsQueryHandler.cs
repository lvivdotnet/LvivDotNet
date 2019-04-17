using Lviv_.NET_Platform.Application.Events.Models;
using Lviv_.NET_Platform.Application.Interfaces;
using Lviv_.NET_Platform.Common;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Lviv_.NET_Platform.Application.Events.Queries.GetEvents
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