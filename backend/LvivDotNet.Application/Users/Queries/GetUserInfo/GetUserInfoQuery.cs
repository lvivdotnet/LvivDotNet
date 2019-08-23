using LvivDotNet.Application.Users.Models;
using MediatR;

namespace LvivDotNet.Application.Users.Queries.GetUserInfo
{
    /// <summary>
    /// Get user information query.
    /// </summary>
    public class GetUserInfoQuery : IRequest<UserInfoModel>
    {
    }
}
