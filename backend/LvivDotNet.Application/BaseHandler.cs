using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using LvivDotNet.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

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
        /// <param name="httpContextAccessor">
        ///     See <see cref="IHttpContextAccessor"/>.
        ///     You should provide this dependency if you want to use property <see cref="User"/>.
        /// </param>
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Injected by DI container.")]
        public BaseHandler(IDbConnectionFactory dbConnectionFactory, IHttpContextAccessor httpContextAccessor = null)
        {
            this.DbConnectionFactory = dbConnectionFactory;
            this.User = httpContextAccessor?.HttpContext?.User;
        }

        /// <summary>
        /// Gets database connection factory.
        /// </summary>
        protected IDbConnectionFactory DbConnectionFactory { get; }

        /// <summary>
        /// Gets the <see cref="ClaimsPrincipal"/> for user associated with the executing action.
        /// </summary>
        protected ClaimsPrincipal User { get; }

        /// <summary>
        /// base request handler.
        /// </summary>
        /// <param name="request"> Request. </param>
        /// <param name="cancellationToken"> Cancellation token. </param>
        /// <returns> Request result. </returns>
        public async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

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
