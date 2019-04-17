using Lviv_.NET_Platform.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Lviv_.NET_Platform.Infrastructure
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string ConnectionString;

        public DbConnectionFactory(IConfiguration configuration)
        {
            ConnectionString = configuration["LvivNetPlatform"];
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
