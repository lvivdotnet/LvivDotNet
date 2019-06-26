using FluentValidation;

namespace LvivDotNet.Application.Users.Commands.Register
{
    /// <summary>
    /// User registration command validation rules.
    /// </summary>
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterUserCommandValidator"/> class.
        /// </summary>
        public RegisterUserCommandValidator()
        {
            this.RuleFor(c => c.FirstName).NotEmpty();
            this.RuleFor(c => c.LastName).NotEmpty();
            this.RuleFor(c => c.Email).EmailAddress().NotEmpty();
            this.RuleFor(c => c.Password).MinimumLength(6).NotEmpty();
        }
    }
}
