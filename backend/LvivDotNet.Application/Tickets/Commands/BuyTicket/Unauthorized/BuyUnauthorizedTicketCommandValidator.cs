using FluentValidation;

namespace LvivDotNet.Application.Tickets.Commands.BuyTicket.Unauthorized
{
    /// <summary>
    /// Buy ticket by unauthorized user command validation rules.
    /// </summary>
    public class BuyUnauthorizedTicketCommandValidator : AbstractValidator<BuyUnauthorizedTicketCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuyUnauthorizedTicketCommandValidator"/> class.
        /// </summary>
        public BuyUnauthorizedTicketCommandValidator()
        {
            this.RuleFor(c => c.EventId).NotNull().GreaterThan(0);
            this.RuleFor(c => c.FirstName).Must(firstName => string.IsNullOrEmpty(firstName) || firstName.Length >= 3);
            this.RuleFor(c => c.LastName).Must(lastName => string.IsNullOrEmpty(lastName) || lastName.Length >= 3);
            this.RuleFor(c => c.Email).EmailAddress();
        }
    }
}
