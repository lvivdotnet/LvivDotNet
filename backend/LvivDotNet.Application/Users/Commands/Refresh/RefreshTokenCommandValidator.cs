using FluentValidation;

namespace LvivDotNet.Application.Users.Commands.Refresh
{
    /// <summary>
    /// Token refresh command validation rules.
    /// </summary>
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenCommandValidator"/> class.
        /// </summary>
        public RefreshTokenCommandValidator()
        {
            this.RuleFor(c => c.JwtToken).NotEmpty();
            this.RuleFor(c => c.RefreshToken).NotEmpty();
        }
    }
}
