using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LvivDotNet.Persistence
{
    /// <summary>
    /// Extensions connected to migration runner.
    /// </summary>
    public static class MigrationsRunnerExtensions
    {
        /// <summary>
        /// Run migrations.
        /// </summary>
        /// <param name="app"> Application builder. </param>
        /// <returns> Returns application builder. </returns>
        public static IApplicationBuilder RunMigrations(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            using (var provider = app.ApplicationServices.CreateScope())
            {
                provider.ServiceProvider.RunMigrations();
            }

            return app;
        }

        /// <summary>
        /// Run migrations.
        /// </summary>
        /// <param name="serviceProvider"> DI container. </param>
        public static void RunMigrations(this IServiceProvider serviceProvider)
        {
            CreateDatabaseIfNotExist(serviceProvider.GetRequiredService<IConfiguration>()["LvivNetPlatform"]);
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        /// <summary>
        /// Add migrations services to DI container.
        /// </summary>
        /// <param name="services"> DI container. </param>
        /// <param name="configuration"> Configuration. </param>
        /// <returns> Returns DI container. </returns>
        public static IServiceCollection AddMigrations(this IServiceCollection services, IConfiguration configuration)
            => services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(configuration["LvivNetPlatform"])
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
                    .AddLogging(lg => lg.AddFluentMigratorConsole());

        /// <summary>
        /// Creates database based on connection string.
        /// </summary>
        /// <param name="connectioString"> Connection string. </param>
        private static void CreateDatabaseIfNotExist(string connectioString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectioString };
            connectionStringBuilder.TryGetValue("Initial Catalog", out var databaseName);
            connectionStringBuilder.Remove("Initial Catalog");

            using (var connection = new SqlConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"select * from master.dbo.sysdatabases where name='{databaseName}'";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            return;
                        }
                    }

                    command.CommandText = $"CREATE DATABASE {databaseName}";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}