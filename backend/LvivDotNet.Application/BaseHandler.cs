using System.Data;
using System.Threading;
using System.Threading.Tasks;
using LvivDotNet.Application.Interfaces;
using MediatR;

namespace LvivDotNet.Application
{
    /// <summary>
    /// Base request handler.
    /// </summary>
    /// <typeparam name="TRequest"> Request type. </typeparam>
    /// <typeparam name="TResult"> Result type. </typeparam>
    public abstract class BaseHandler<TRequest, TResult> : IRequestHandler<TRequest, TResult>
        where TRequest : IRequest<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseHandler{TRequest, TResult}"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        public BaseHandler(IDbConnectionFactory dbConnectionFactory)
        {
            this.DbConnectionFactory = dbConnectionFactory;
        }

        /// <summary>
        /// Gets database connection factory.
        /// </summary>
        protected IDbConnectionFactory DbConnectionFactory { get; }

        /// <summary>
        /// base request handler.
        /// </summary>
        /// <param name="request"> Request. </param>
        /// <param name="cancellationToken"> Cancellation token. </param>
        /// <returns> Request result. </returns>
        public async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken)
        {
            using (var connection = this.DbConnectionFactory.Connection)
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var result = await this.Handle(request, connection, transaction, cancellationToken);

                    transaction.Commit();
                    connection.Close();

                    return result;
                }
            }
        }

        /// <summary>
        /// Handle request.
        /// </summary>
        /// <param name="request"> Request. </param>
        /// <param name="connection"> Database connection. </param>
        /// <param name="transaction"> Database transaction. </param>
        /// <param name="cancellationToken"> Cancellation token. </param>
        /// <returns> Result. </returns>
        protected abstract Task<TResult> Handle(TRequest request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken);
    }
}
