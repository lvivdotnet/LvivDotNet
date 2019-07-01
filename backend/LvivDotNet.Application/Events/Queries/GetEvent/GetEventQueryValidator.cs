using FluentValidation;

namespace LvivDotNet.Application.Events.Queries.GetEvent
{
    /// <summary>
    /// Get event query validation rules.
    /// </summary>
    public class GetEventQueryValidator : AbstractValidator<GetEventQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetEventQueryValidator"/> class.
        /// </summary>
        public GetEventQueryValidator()
        {
            this.RuleFor(c => c.EventId).NotEmpty().GreaterThan(0);
        }
    }
}
