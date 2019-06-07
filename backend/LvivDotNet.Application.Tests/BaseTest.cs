using LvivDotNet.Application.Events.Commands.AddEvent;
using LvivDotNet.Application.Infrastructure;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Infrastructure;
using LvivDotNet.Persistence;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LvivDotNet.Application.Tests
{
    public abstract class BaseTest
    {
        protected IServiceProvider ServiceProvider;

        public BaseTest()
        {
            var configurationProviders = new List<IConfigurationProvider>
            {
                new MemoryConfigurationSource
                {
                    InitialData = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("LvivNetPlatform", Environment.GetEnvironmentVariable("LvivNetPlatform"))
                    }
                }.Build(new ConfigurationBuilder())
            };
            var configuration = new ConfigurationRoot(configurationProviders);

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddMigrations(configuration);
            services.AddMediatR(typeof(AddEventCommandHandler).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();

            ServiceProvider = services.BuildServiceProvider();

            ServiceProvider.RunMigrations();
        }
    }
}


