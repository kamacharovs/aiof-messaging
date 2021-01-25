using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;

namespace aiof.messaging.data
{
    public class EnvConfiguration : IEnvConfiguration
    {
        public readonly IConfiguration _config;
        public readonly IFeatureManager _featureManager;

        public EnvConfiguration(
            IConfiguration config,
            IFeatureManager featureManager)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _featureManager = featureManager;
        }
        
        public string ServiceBusConnectionString => _config[Keys.ServiceBusConnectionString] ?? throw new KeyNotFoundException();
        public string EmailQueueName => _config[Keys.EmailQueueName] ?? throw new KeyNotFoundException();
        public string EmailTableName => _config[Keys.EmailTableName] ?? throw new KeyNotFoundException();
        public string InboundQueueName => _config[Keys.InboundQueueName] ?? throw new KeyNotFoundException();

        public async Task<bool> IsEnabledAsync(FeatureFlags featureFlag)
        {
            return await _featureManager.IsEnabledAsync(featureFlag.ToString());
        }
    }

    public enum FeatureFlags
    {
        Email
    }
}
