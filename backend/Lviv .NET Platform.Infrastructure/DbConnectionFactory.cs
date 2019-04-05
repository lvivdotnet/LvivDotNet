using System.Data;
using System.Data.SqlClient;
using Lviv_.NET_Platform.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Lviv_.NET_Platform.Infrastructure
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string ConnectionString;

        public DbConnectionFactory(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("LvivNetPlatform");
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
