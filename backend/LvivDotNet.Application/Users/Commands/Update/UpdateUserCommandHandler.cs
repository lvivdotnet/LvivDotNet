using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Exceptions;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.Users.Models;
using LvivDotNet.Common.Extensions;
using Microsoft.AspNetCore.Http;

namespace LvivDotNet.Application.Users.Commands.Update
{
    /// <summary>
    /// Update user command handler.
    /// </summary>
    public class UpdateUserCommandHandler : BaseHandler<UpdateUserCommand, UserInfoModel>
    {
        /// <summary>
        /// Check if user exist sql query.
        /// </summary>
        private const string CheckIfUserExistQuery =
            @"select 1 from public.user where ""user"".""Id"" = @Id limit 1";

        /// <summary>
        /// Update user sql command.
        /// </summary>
        private const string UpdateUserCommand =
            @"update public.""user"" " +
            @"set ""FirstName"" = @FirstName, " +
            @"""LastName"" = @LastName, " +
            @"""Email"" = @Email, " +
            @"""Phone"" = @Phone, " +
            @"""Sex"" = @Sex, " +
            @"""Age"" = @Age, " +
            @"""Avatar"" = @Avatar " +
            @"where ""Id"" = cast(@Id as integer);" +
            @"select ""user"".""FirstName"", ""user"".""LastName"", ""user"".""Email"", ""user"".""Phone"", ""user"".""Sex"", " +
            @"""user"".""Avatar"", ""user"".""Age"", ""role"".""Name"" as RoleName from public.user " +
            @"join public.role on  ""role"".""Id"" = ""user"".""RoleId"" " +
            @"where ""user"".""Id"" = @Id;";

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserCommandHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        /// <param name="httpContextAccessor"> See <see cref="IHttpContextAccessor"/>. </param>
        public UpdateUserCommandHandler(IDbConnectionFactory dbConnectionFactory, IHttpContextAccessor httpContextAccessor)
            : base(dbConnectionFactory, httpContextAccessor)
        {
        }

        /// <inheritdoc/>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "We already have a not-null check for request in MediatR")]
        protected override async Task<UserInfoModel> Handle(UpdateUserCommand request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            var userExist = await connection.QuerySingleAsync<bool>(CheckIfUserExistQuery, new { Id = this.User.GetId() }, transaction);

            if (!userExist)
            {
                throw new NotFoundException("User", this.User.GetId());
            }

            return await connection.QuerySingleAsync<UserInfoModel>(
                UpdateUserCommand,
                new
                {
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Phone,
                    request.Sex,
                    request.Age,
                    request.Avatar,
                    Id = this.User.GetId(),
                },
                transaction);
        }
    }
}
