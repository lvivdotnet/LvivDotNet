using FluentValidation;

namespace LvivDotNet.Application.Tickets.Commands.BuyTicket.Authorized
{
    /// <summary>
    /// Buy ticket command for authorized users validation rules.
    /// </summary>
    public class BuyAuthorizedTicketCommandValidator : AbstractValidator<BuyAuthorizedTicketCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuyAuthorizedTicketCommandValidator"/> class.
        /// </summary>
        public BuyAuthorizedTicketCommandValidator()
        {
            this.RuleFor(c => c.EventId).GreaterThan(0);
        }
    }
}
