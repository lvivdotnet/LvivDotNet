using FluentValidation;

namespace LvivDotNet.Application.Users.Commands.Logout
{
    /// <summary>
    /// Logout command validation rules.
    /// </summary>
    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogoutCommandValidator"/> class.
        /// </summary>
        public LogoutCommandValidator()
        {
            this.RuleFor(c => c.Token).NotEmpty();
            this.RuleFor(c => c.RefreshToken).NotEmpty();
        }
    }
}
