using FluentValidation;

namespace LvivDotNet.Application.Tickets.Queries.GetTicket
{
    /// <summary>
    /// Get ticket by id query validation rules.
    /// </summary>
    public class GetTicketQueryValidator : AbstractValidator<GetTicketQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetTicketQueryValidator"/> class.
        /// </summary>
        public GetTicketQueryValidator()
        {
            this.RuleFor(c => c.TicketId).NotNull().GreaterThan(0);
            this.RuleFor(c => c.UserId).NotNull().GreaterThan(0);
        }
    }
}
