﻿using System.Data;
using LvivDotNet.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace LvivDotNet.Infrastructure
{
    /// <inheritdoc />
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionFactory"/> class.
        /// </summary>
        /// <param name="configuration"> Configuration. </param>
        public DbConnectionFactory(IConfiguration configuration)
        {
            if (configuration != null)
            {
                this.connectionString = configuration["LvivNetPlatform"];
            }
        }

        /// <inheritdoc />
        public IDbConnection Connection => new NpgsqlConnection(this.connectionString);
    }
}
