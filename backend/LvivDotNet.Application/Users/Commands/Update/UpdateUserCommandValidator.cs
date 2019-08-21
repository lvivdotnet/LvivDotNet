using FluentValidation;

namespace LvivDotNet.Application.Users.Commands.Update
{
    /// <summary>
    /// Update user command validation rules.
    /// </summary>
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserCommandValidator"/> class.
        /// </summary>
        public UpdateUserCommandValidator()
        {
            this.RuleFor(c => c.Id).GreaterThan(0);
            this.RuleFor(c => c.FirstName).NotEmpty();
            this.RuleFor(c => c.LastName).NotEmpty();
            this.RuleFor(c => c.Email).EmailAddress().NotEmpty();
        }
    }
}
