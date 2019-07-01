using FluentValidation;

namespace LvivDotNet.Application.Tickets.Commands.BuyTicket.Authorized
{
    /// <summary>
    /// Buy ticket command for authorized users validation rules.
    /// </summary>
    public class BuyTicketCommandValidator : AbstractValidator<BuyTicketCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuyTicketCommandValidator"/> class.
        /// </summary>
        public BuyTicketCommandValidator()
        {
            this.RuleFor(c => c.EventId).GreaterThan(0);
            this.RuleFor(c => c.UserEmail).EmailAddress();
        }
    }
}
