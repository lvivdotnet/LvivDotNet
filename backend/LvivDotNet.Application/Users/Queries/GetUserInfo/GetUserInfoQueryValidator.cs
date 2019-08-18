using FluentValidation;

namespace LvivDotNet.Application.Users.Queries.GetUserInfo
{
    /// <summary>
    /// Get user info validation rules.
    /// </summary>
    public class GetUserInfoQueryValidator : AbstractValidator<GetUserInfoQuery>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserInfoQueryValidator"/> class.
        /// </summary>
        public GetUserInfoQueryValidator()
        {
            this.RuleFor(c => c.UserId).GreaterThan(0);
        }
    }
}
