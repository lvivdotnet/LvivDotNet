using Lviv_.NET_Platform.Application.Interfaces;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Lviv_.NET_Platform.Application
{
    public abstract class BaseHandler<TRequest, TResult> : IRequestHandler<TRequest, TResult>
        where TRequest : IRequest<TResult>
    {
        protected readonly IDbConnectionFactory dbConnectionFactory;

        public BaseHandler(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken)
        {
            using (var connection = dbConnectionFactory.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var result = await Handle(request, cancellationToken, connection, transaction);

                    transaction.Commit();
                    connection.Close();

                    return result;
                }
            }
        }

        protected abstract Task<TResult> Handle(TRequest request, CancellationToken cancellationToken, IDbConnection connection, IDbTransaction transaction);
    }

    public abstract class BaseHandler<TRequest> : BaseHandler<TRequest, Unit>
        where TRequest : IRequest<Unit>
    {
        public BaseHandler(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }
    }
}
