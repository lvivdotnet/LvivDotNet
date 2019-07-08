using FluentValidation;

namespace LvivDotNet.Application.Tickets.Queries.GetUserTickets
{
    /// <summary>
    /// Gets all user tickets query validation rules.
    /// </summary>
    public class GetUserTicketsQueryValidator : AbstractValidator<GetUserTicketsQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserTicketsQueryValidator"/> class.
        /// </summary>
        public GetUserTicketsQueryValidator()
        {
            this.RuleFor(c => c.UserId).NotEmpty().GreaterThan(0);
        }
    }
}
