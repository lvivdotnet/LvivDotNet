using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace LvivDotNet.Application.Infrastructure
{
    /// <summary>
    /// Mediator validation request behavior.
    /// </summary>
    /// <typeparam name="TRequest"> Request type. </typeparam>
    /// <typeparam name="TResponse"> Response type. </typeparam>
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestValidationBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="validators"> Collection of fluent validation rules. </param>
        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        /// <inheritdoc/>
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            var context = new ValidationContext(request);

            var failures = this.validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            return next();
        }
    }
}
