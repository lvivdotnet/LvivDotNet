using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace Lviv_.NET_Platform.Persistence
{
    public static class MigrationsRunnerExtensions
    {
        public static async Task<IApplicationBuilder> RunMigrations(this IApplicationBuilder app)
        {
            using (var provider = app.ApplicationServices.CreateScope())
            {
                await provider.ServiceProvider.RunMigrations();
            }

            return app;
        }

        public static async Task RunMigrations(this IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            var logger = serviceProvider.GetRequiredService<ILogger<IMigrationRunner>>();
            await CreateDatabaseIfNotExist(serviceProvider.GetRequiredService<IConfiguration>(), logger);
            runner.MigrateUp();
        }

        public static IServiceCollection AddMigrations(this IServiceCollection services, IConfiguration configuration)
            => services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(configuration["LvivNetPlatform"])
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
                    .AddLogging(lg => lg.AddFluentMigratorConsole());

        private static async Task CreateDatabaseIfNotExist(IConfiguration configuration, ILogger logger)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = configuration["LvivNetPlatform"] };
            connectionStringBuilder.TryGetValue("Initial Catalog", out var databaseName);
            connectionStringBuilder.Remove("Initial Catalog");

            using (var connection = new SqlConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = string.Format("select * from master.dbo.sysdatabases where name='{0}'", databaseName);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            logger.LogInformation($"The database {databaseName} already exists");
                            return;
                        }
                    }

                    command.CommandText = string.Format("CREATE DATABASE {0}", databaseName);
                    await command.ExecuteNonQueryAsync();
                    logger.LogInformation($"The database {databaseName} created");
                }
            }
        }
    }
}