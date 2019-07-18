using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LvivDotNet.Application.Infrastructure
{
    /// <summary>
    /// Mediator performance checking pipeline behavior.
    /// </summary>
    /// <typeparam name="TRequest"> Request type. </typeparam>
    /// <typeparam name="TResponse"> Response type. </typeparam>
    public class RequestPerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch timer;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestPerformanceBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="logger"> <see cref="ILogger{TRequest}"/>. </param>
        public RequestPerformanceBehavior(ILogger<TRequest> logger)
        {
            this.timer = new Stopwatch();
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (next == null)
            {
                throw new NullReferenceException(nameof(next));
            }

            var name = typeof(TRequest).Name;

            this.logger.LogInformation("Request Start: {Name} {@Request}", name, request);

            this.timer.Start();

            var response = await next();

            this.timer.Stop();

            if (this.timer.ElapsedMilliseconds > 500)
            {
                this.logger.LogWarning("LvivDotNet Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}", name, this.timer.ElapsedMilliseconds, request);
            }
            else
            {
                this.logger.LogInformation("Request End: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}", name, this.timer.ElapsedMilliseconds, request);
            }

            return response;
        }
    }
}
