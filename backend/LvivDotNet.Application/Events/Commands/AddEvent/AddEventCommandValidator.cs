using FluentValidation;

namespace LvivDotNet.Application.Events.Commands.AddEvent
{
    /// <summary>
    /// Add event command validation rule.
    /// </summary>
    public class AddEventCommandValidator : AbstractValidator<AddEventCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddEventCommandValidator"/> class.
        /// </summary>
        public AddEventCommandValidator()
        {
            this.RuleFor(c => c.Name).NotEmpty();
            this.RuleFor(c => c.StartDate).NotEmpty().LessThan(c => c.EndDate);
            this.RuleFor(c => c.EndDate).NotEmpty().GreaterThan(c => c.StartDate);
            this.RuleFor(c => c.Address).NotEmpty();
            this.RuleFor(c => c.Title).NotEmpty();
            this.RuleFor(c => c.Description).NotEmpty();
            this.RuleFor(c => c.MaxAttendees).NotEmpty().GreaterThan(0);
        }
    }
}
