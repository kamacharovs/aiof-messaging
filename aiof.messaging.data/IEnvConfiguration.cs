using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aiof.messaging.data
{
    public interface IEnvConfiguration
    {
        string ServiceBusConnectionString { get; }
        string DatabaseConnectionString { get; }
        string StorageConnectionString { get; }
        string EmailQueueName { get; }
        string EmailTableName { get; }
        string InboundQueueName { get; }
        string InboundTableName { get; }
        string DeadLetterTableName { get; }

        Task<bool> IsEnabledAsync(FeatureFlags featureFlag);
    }
}
