using FluentValidation;

namespace LvivDotNet.Application.Events.Commands.AddEvent
{
    public class AddEventCommandValidator : AbstractValidator<AddEventCommand>
    {
        public AddEventCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.StartDate).NotEmpty().LessThan(c => c.EndDate);
            RuleFor(c => c.EndDate).NotEmpty().GreaterThan(c => c.StartDate);
            RuleFor(c => c.Address).NotEmpty();
            RuleFor(c => c.Title).NotEmpty();
            RuleFor(c => c.Description).NotEmpty();
            RuleFor(c => c.MaxAttendees).NotEmpty().GreaterThan(0);
        }
    }
}
