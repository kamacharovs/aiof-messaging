using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

using aiof.messaging.data;
using aiof.messaging.services;

[assembly: FunctionsStartup(typeof(aiof.messaging.function.Startup))]
namespace aiof.messaging.function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
        }
    }
}
