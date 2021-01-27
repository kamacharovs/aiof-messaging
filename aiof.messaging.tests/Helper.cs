using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos.Table;
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
            { "StorageConnectionString", "DefaultEndpointsProtocol=https;AccountName=local;AccountKey=acckey==;EndpointSuffix=core.windows.net" },
            { "EmailQueueName", "email" },
            { "EmailTableName", "email" },
            { "InboundQueueName", "inbound" },
            { "InboundTableName", "inbound" },
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
                .AddSingleton(CloudStorageAccount.Parse(ConfigurationDict[Keys.StorageConnectionString]).CreateCloudTableClient(new TableClientConfiguration()))
                .AddSingleton<IEnvConfiguration, EnvConfiguration>();

            services.AddSingleton(new MapperConfiguration(x => { x.AddProfile(new AutoMappingProfile()); }).CreateMapper());

            services
                .AddScoped<FakeDataManager>()
                .AddScoped<AbstractValidator<IMessage>, MessageValidator>()
                .AddScoped<AbstractValidator<IEmailMessage>, EmailMessageValidator>()
                .AddScoped<ITestConfigRepository, TestConfigRepository>()
                .AddScoped(x => GetMockedTableRepository())
                .AddScoped(x => GetMockedMessageRepository());

            services.AddLogging();
            services.AddFeatureManagement();
            services.AddMemoryCache();

            return services.BuildServiceProvider();
        }

        public static IMessageRepository GetMockedMessageRepository()
        {
            return GetMockMessageRepository().Object;
        }
        public static Mock<IMessageRepository> GetMockMessageRepository()
        {
            var repo = new Mock<IMessageRepository>();

            repo.Setup(x => x.SendInboundMessageAsync(It.IsAny<IMessage>()))
                .Verifiable();
            repo.Setup(x => x.SendEmailMessageAsync(It.IsAny<IEmailMessage>()))
                .Verifiable();
            repo.Setup(x => x.SendMessageAsync(It.IsAny<IMessage>()))
                .Verifiable();
            repo.Setup(x => x.SendAsync(It.IsAny<IMessage>()))
                .Verifiable();

            return repo;
        }

        public static ITableRepository GetMockedTableRepository()
        {
            return GetMockTableRepository().Object;
        }
        public static Mock<ITableRepository> GetMockTableRepository()
        {
            var repo = new Mock<ITableRepository>();

            repo.Setup(x => x.LogAsync(It.IsAny<IMessage>()))
                .Verifiable();
            repo.Setup(x => x.LogAsync(It.IsAny<IEmailMessage>()))
                .Verifiable();
            repo.Setup(x => x.InsertAsync(It.IsAny<string>(), It.IsAny<TableEntity>()))
                .Verifiable();

            return repo;
        }

        public const string Category = nameof(Category);
        public const string UnitTest = nameof(UnitTest);
        public const string IntegrationTest = nameof(IntegrationTest);

        public static string NTimes(int n) { return new String('\t', n); }
        public static string MoreThanOneHundred => NTimes(101);
    }
}
