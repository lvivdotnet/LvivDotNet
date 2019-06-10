using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LvivDotNet.Application.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        public UpdateEventCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty().NotEqual(0);
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
