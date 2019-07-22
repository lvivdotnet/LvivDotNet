using FluentValidation;

namespace LvivDotNet.Application.Users.Commands.Login
{
    /// <summary>
    /// Login command validation rules.
    /// </summary>
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommandValidator"/> class.
        /// </summary>
        public LoginCommandValidator()
        {
            this.RuleFor(c => c.Email).NotEmpty().EmailAddress();
            this.RuleFor(c => c.Password).MinimumLength(6).NotEmpty();
        }
    }
}
