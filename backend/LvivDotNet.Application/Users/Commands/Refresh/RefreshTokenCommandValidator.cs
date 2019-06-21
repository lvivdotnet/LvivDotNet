using FluentValidation;

namespace LvivDotNet.Application.Users.Commands.Refresh
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(c => c.JwtToken).NotEmpty();
            RuleFor(c => c.RefreshToken).NotEmpty();
        }
    }
}
