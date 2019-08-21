using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Exceptions;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.Users.Models;
using LvivDotNet.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LvivDotNet.Application.Users.Queries.GetUserInfo
{
    /// <summary>
    /// Get user information query handler.
    /// </summary>
    public class GetUserInfoQueryHandler : BaseHandler<GetUserInfoQuery, UserInfoModel>
    {
        private const string GetUserInfoSqlQuery =
            @"select ""user"".""FirstName"", ""user"".""LastName"", ""user"".""Email"", ""user"".""Phone"", ""user"".""Sex"", " +
            @"""user"".""Avatar"", ""user"".""Age"", ""role"".""Name"" as RoleName from public.user " +
            @"join public.role on  ""role"".""Id"" = ""user"".""RoleId"" " +
            @"where ""user"".""Id"" = @UserId";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserInfoQueryHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        /// <param name="httpContextAccessor"> See <see cref="IHttpContextAccessor"/>. </param>
        public GetUserInfoQueryHandler(IDbConnectionFactory dbConnectionFactory, IHttpContextAccessor httpContextAccessor)
            : base(dbConnectionFactory, httpContextAccessor)
        {
        }

        /// <inheritdoc/>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "We already have a not-null check for request in MediatR")]
        protected override async Task<UserInfoModel> Handle(GetUserInfoQuery request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            var userId = this.User.GetId();
            var user = await connection.QueryFirstAsync<UserInfoModel>(GetUserInfoSqlQuery, new { UserId = userId }, transaction);

            if (user == null)
            {
                throw new NotFoundException("User", userId);
            }

            return user;
        }
    }
}
