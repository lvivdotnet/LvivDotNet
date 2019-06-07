using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace LvivDotNet.Persistence
{
    public static class MigrationsRunnerExtensions
    {
        public static IApplicationBuilder RunMigrations(this IApplicationBuilder app)
        {
            using (var provider = app.ApplicationServices.CreateScope())
            {
                provider.ServiceProvider.RunMigrations();
            }

            return app;
        }

        public static void RunMigrations(this IServiceProvider serviceProvider)
        {
            CreateDatabaseIfNotExist(serviceProvider.GetRequiredService<IConfiguration>());
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
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

        private static void CreateDatabaseIfNotExist(IConfiguration configuration)
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
                            return;
                        }
                    }

                    command.CommandText = string.Format("CREATE DATABASE {0}", databaseName);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}