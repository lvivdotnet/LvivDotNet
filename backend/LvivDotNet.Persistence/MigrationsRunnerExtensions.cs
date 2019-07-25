using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

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
            var connectionString = serviceProvider.GetRequiredService<IConfiguration>()["LvivNetPlatform"];
            WaitForDatabase(connectionString);
            CreateDatabaseIfNotExist(connectionString);
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
                    .AddPostgres()
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
            connectionStringBuilder.TryGetValue("Database", out var databaseName);
            connectionStringBuilder.Remove("Database");

            using (var connection = new NpgsqlConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"select * from pg_database where datname='{databaseName}'";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            return;
                        }
                    }

                    command.CommandText = $"create database {databaseName}";
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Wait for database to become available with retry count.
        /// </summary>
        /// <param name="connectioString"> Database connection string. </param>
        /// <param name="retryCount"> Maximum retry count. </param>
        private static void WaitForDatabase(string connectioString, int retryCount = 60)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectioString };
            connectionStringBuilder.Remove("Database");

            var retry = 0;

            using (var connection = new NpgsqlConnection(connectionStringBuilder.ConnectionString))
            {
                while (retry < retryCount)
                {
                    try
                    {
                        connection.Open();
                        return;
                    }
                    catch (SqlException exception)
                    {
                        retry++;

                        var message = GetMessage(exception);

                        Console.WriteLine(message);
                        Task.Delay(250);
                    }
                }
            }

            string GetMessage(SqlException exception) => $"Can`t create connection to database. Error: {exception.Message}. Connection string: {connectionStringBuilder.ConnectionString}";
        }
    }
}