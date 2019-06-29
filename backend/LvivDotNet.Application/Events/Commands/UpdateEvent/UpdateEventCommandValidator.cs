using FluentValidation;

namespace LvivDotNet.Application.Events.Commands.UpdateEvent
{
    /// <summary>
    /// Update event command validation rules.
    /// </summary>
    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateEventCommandValidator"/> class.
        /// </summary>
        public UpdateEventCommandValidator()
        {
            this.RuleFor(c => c.Id).NotEmpty().NotEqual(0);
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
