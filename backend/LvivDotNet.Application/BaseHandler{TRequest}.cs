using LvivDotNet.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LvivDotNet.Application
{
    /// <summary>
    /// Base request handler.
    /// </summary>
    /// <typeparam name="TRequest"> Request type. </typeparam>
    public abstract class BaseHandler<TRequest> : BaseHandler<TRequest, Unit>
        where TRequest : IRequest<Unit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseHandler{TRequest}"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        /// <param name="httpContextAccessor">
        ///     See <see cref="IHttpContextAccessor"/>.
        ///     You should provide this dependency if you want to use property <see cref="BaseHandler{TRequest, Unit}.User"/>.
        /// </param>
        public BaseHandler(IDbConnectionFactory dbConnectionFactory, IHttpContextAccessor httpContextAccessor = null)
            : base(dbConnectionFactory, httpContextAccessor)
        {
        }
    }
}
