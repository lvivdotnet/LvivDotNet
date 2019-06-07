using FluentValidation;

namespace LvivDotNet.Application.Users.Commands.Logout
{
    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.RefreshToken).NotEmpty();
        }
    }
}
