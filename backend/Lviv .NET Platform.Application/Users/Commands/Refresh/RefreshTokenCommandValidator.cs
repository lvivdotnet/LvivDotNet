using FluentValidation;

namespace Lviv_.NET_Platform.Application.Users.Commands.Refresh
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(c => c.JwnToken).NotEmpty();
            RuleFor(c => c.RefreshToken).NotEmpty();
        }
    }
}
