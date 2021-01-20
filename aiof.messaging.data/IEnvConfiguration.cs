﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aiof.messaging.data
{
    public interface IEnvConfiguration
    {
        string ServiceBusConnectionString { get; }
        string InboundQueueName { get; }
        string EmailQueueName { get; }

        Task<bool> IsEnabledAsync(FeatureFlags featureFlag);
    }
}