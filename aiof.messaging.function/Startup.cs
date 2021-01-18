using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

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

            //builder.Services.AddSingleton(new QueueClient(
            //    new ServiceBusConnectionStringBuilder(_config[Keys.ServiceBusConnectionString]),
            //    ReceiveMode.PeekLock));

            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
        }
    }
}
