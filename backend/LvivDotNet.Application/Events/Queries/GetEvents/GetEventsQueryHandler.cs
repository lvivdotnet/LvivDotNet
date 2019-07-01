using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Events.Models;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Common;

namespace LvivDotNet.Application.Events.Queries.GetEvents
{
    /// <summary>
    /// Get events query handler.
    /// </summary>
    public class GetEventsQueryHandler : BaseHandler<GetEventsQuery, Page<EventShortModel>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetEventsQueryHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        public GetEventsQueryHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        /// <inheritdoc/>
        protected override async Task<Page<EventShortModel>> Handle(GetEventsQuery request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            var events = await connection.QueryAsync<EventShortModel>(
                "select * from dbo.[event] " +
                "order by [Id]" +
                "offset @Skip rows fetch next @Take rows only",
                request,
                transaction)
                .ConfigureAwait(false);

            return new Page<EventShortModel>
            {
                Items = events,
                Total = events.Count(),
            };
        }
    }
}