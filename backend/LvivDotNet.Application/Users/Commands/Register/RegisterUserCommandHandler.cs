using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.Users.Models;
using LvivDotNet.Common;
using Microsoft.Extensions.Configuration;

namespace LvivDotNet.Application.Users.Commands.Register
{
    /// <summary>
    /// User registration command handler.
    /// </summary>
    public class RegisterUserCommandHandler : BaseHandler<RegisterUserCommand, AuthTokensModel>
    {
        /// <summary>
        /// Insert user sql command.
        /// </summary>
        private const string InsertUserSqlCommand =
                @"insert into public.user(""FirstName"", ""LastName"", ""Email"", ""Phone"", ""Sex"", ""Age"", ""Avatar"", ""Password"", ""Salt"", ""RoleId"") " +
                @"values (@FirstName, @LastName, @Email, @Phone, @Sex, @Age, @Avatar, @Password, @Salt, (select ""Id"" from public.role where ""Name"" = 'User' limit 1)) " +
                @"returning ""Id"";";

        /// <summary>
        /// Insert refresh sql command.
        /// </summary>
        private const string InsertRefreshSqlToken =
            @"insert into public.refresh_token(""UserId"", ""RefreshToken"", ""Expires"") values (@UserId, @RefreshToken, @Expires)";

        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterUserCommandHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        /// <param name="configuration"> Configuration. </param>
        public RegisterUserCommandHandler(IDbConnectionFactory dbConnectionFactory, IConfiguration configuration)
            : base(dbConnectionFactory)
        {
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "We already have a not-null check for request in MediatR")]
        protected override async Task<AuthTokensModel> Handle(RegisterUserCommand request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            var salt = SecurityHelpers.GetRandomBytes(32);

            var passwordHash = SecurityHelpers.GetPasswordHash(request.Password, salt);

            var refreshToken = Convert.ToBase64String(SecurityHelpers.GetRandomBytes(32));

            var insertUserParams = new
                {
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Phone,
                    request.Sex,
                    request.Age,
                    request.Avatar,
                    Password = passwordHash,
                    Salt = Convert.ToBase64String(salt),
                };
            var userId = await connection.QuerySingleAsync<int>(InsertUserSqlCommand, insertUserParams, transaction)
                .ConfigureAwait(false);

            await connection.ExecuteAsync(InsertRefreshSqlToken, new { UserId = userId, RefreshToken = refreshToken, Expires = DateTime.UtcNow.AddDays(14) }, transaction)
                .ConfigureAwait(false);

            var token = SecurityHelpers.GenerateJwtToken(userId, this.configuration["Secret"], "User");

            return new AuthTokensModel
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                RefreshToken = refreshToken,
                JwtToken = token,
                Role = "User",
            };
        }
    }
}