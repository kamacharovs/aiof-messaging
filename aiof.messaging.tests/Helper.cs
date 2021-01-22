using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.EntityFrameworkCore;
using Azure.Messaging.ServiceBus;

using FluentValidation;
using AutoMapper;

using aiof.messaging.data;
using aiof.messaging.services;

namespace aiof.messaging.tests
{
    public static class Helper
    {
        public static Dictionary<string, string> ConfigurationDict
            => new Dictionary<string, string>()
        {
            { "AzureWebJobsStorage", "UseDevelopmentStorage" },
            { "ServiceBusConnectionString", "" },
            { "DatabaseConnectionString", "" },
            { "EmailQueueName", "email" },
            { "InboundQueueName", "inbound" },
            { "FUNCTIONS_WORKER_RUNTIME", "dotnet" },
            { "FeatureManagement:Email", "true" },
        };

        private static IServiceProvider Provider()
        {
            var services = new ServiceCollection();

            services.AddDbContext<MessageContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services.AddScoped<IConfiguration>(x =>
            {
                IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
                configurationBuilder.AddInMemoryCollection(ConfigurationDict);
                configurationBuilder.AddEnvironmentVariables();
                return configurationBuilder.Build();
            });

            services
                .AddSingleton(new ServiceBusClient(ConfigurationDict[Keys.ServiceBusConnectionString]))
                .AddSingleton<IEnvConfiguration, EnvConfiguration>();

            services.AddSingleton(new MapperConfiguration(x => { x.AddProfile(new AutoMappingProfile()); }).CreateMapper());

            services
                .AddScoped<FakeDataManager>()
                .AddScoped<AbstractValidator<IMessage>, MessageValidator>()
                .AddScoped<AbstractValidator<IEmailMessage>, EmailMessageValidator>()
                .AddScoped<IMessageRepository, MessageRepository>()
                .AddScoped<ITestConfigRepository, TestConfigRepository>();

            services.AddLogging();
            services.AddFeatureManagement();
            services.AddMemoryCache();

            return services.BuildServiceProvider();
        }
    }
}
