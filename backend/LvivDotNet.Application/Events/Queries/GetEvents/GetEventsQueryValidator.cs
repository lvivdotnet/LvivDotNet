using FluentValidation;

namespace LvivDotNet.Application.Events.Queries.GetEvents
{
    /// <summary>
    /// Get events query validation rules.
    /// </summary>
    public class GetEventsQueryValidator : AbstractValidator<GetEventsQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetEventsQueryValidator"/> class.
        /// </summary>
        public GetEventsQueryValidator()
        {
            this.RuleFor(c => c.Skip).GreaterThan(0);
            this.RuleFor(c => c.Take).GreaterThan(0);
        }
    }
}