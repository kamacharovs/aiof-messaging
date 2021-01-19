using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using Azure.Messaging.ServiceBus;

using aiof.messaging.data;
using aiof.messaging.services;

[assembly: FunctionsStartup(typeof(aiof.messaging.function.Startup))]
namespace aiof.messaging.function
{
    public class Startup : FunctionsStartup
    {
        private IConfiguration _config { get; set; }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            _config = builder.GetContext().Configuration;

            builder.Services.AddSingleton(_config);
            builder.Services.AddSingleton(new ServiceBusClient(_config[Keys.ServiceBusConnectionString]));

            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
        }
    }
}
