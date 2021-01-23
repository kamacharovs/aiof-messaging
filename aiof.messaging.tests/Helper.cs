using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.EntityFrameworkCore;
using Azure.Messaging.ServiceBus;

using FluentValidation;
using AutoMapper;
using Moq;

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
            { "ServiceBusConnectionString", "Endpoint=sb://local.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=somefakeone=" },
            { "DatabaseConnectionString", "Server=127.0.0.1;Port=5433;Database=aiof;User Id=aiof;Password=aiofiscool;" },
            { "EmailQueueName", "email" },
            { "InboundQueueName", "inbound" },
            { "FUNCTIONS_WORKER_RUNTIME", "dotnet" },
            { "FeatureManagement:Email", "true" },
        };

        public static T GetRequiredService<T>()
        {
            var provider = Provider();

            provider.GetRequiredService<FakeDataManager>()
                .UseFakeContext();

            return provider.GetRequiredService<T>();
        }

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

        public static IMessageRepository GetMockedMessageRepository()
        {
            var repo = new Mock<IMessageRepository>();

            repo.Setup(x => x.SendInboundMessageAsync(It.IsAny<IMessage>()))
                .Verifiable();
            repo.Setup(x => x.SendEmailMessageAsync(It.IsAny<IEmailMessage>()))
                .Verifiable();
            repo.Setup(x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<object>()))
                .Verifiable();
            repo.Setup(x => x.SendAsync(It.IsAny<IMessage>()))
                .Verifiable();

            return repo.Object;
        }

        public const string Category = nameof(Category);
        public const string UnitTest = nameof(UnitTest);
        public const string IntegrationTest = nameof(IntegrationTest);

        public static string NTimes(int n) { return new String('\t', n); }
        public static string MoreThanOneHundred => NTimes(101);
    }
}