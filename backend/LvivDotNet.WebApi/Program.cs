using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace LvivDotNet
{
    /// <summary>
    /// Entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        public static void Main()
        {
            CreateWebHostBuilder().Build().Run();
        }

        /// <summary>
        /// Creates WbHostBuilder.
        /// </summary>
        /// <returns> Web host builder. </returns>
        public static IWebHostBuilder CreateWebHostBuilder() =>
            new WebHostBuilder()
                .UseConfiguration(
                    new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .Build())
                .UseStartup<Startup>()
                .ConfigureLogging(LoggingSetup)
                .UseKestrel();

        /// <summary>
        /// Default logging setup for Elasticsearch or Console logging.
        /// </summary>
        /// <param name="context"> <see cref="WebHostBuilderContext"/>. </param>
        /// <param name="loggingBuilder"> <see cref="ILoggingBuilder"/>. </param>
        private static void LoggingSetup(WebHostBuilderContext context, ILoggingBuilder loggingBuilder)
        {
            if (Uri.TryCreate(context.Configuration["elastic"], UriKind.RelativeOrAbsolute, out var elasticsearchAddress))
            {
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(elasticsearchAddress))
                    .CreateLogger();

                loggingBuilder.AddSerilog();

                return;
            }

            loggingBuilder.AddConsole();
        }
    }
}
