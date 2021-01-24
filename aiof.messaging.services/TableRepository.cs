using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Azure.Cosmos.Table;

using aiof.messaging.data;

namespace aiof.messaging.services
{
    public class TableRepository : ITableRepository
    {
        private readonly CloudStorageAccount _account;

        public TableRepository(CloudStorageAccount account)
        {
            _account = account ?? throw new ArgumentNullException(nameof(account));
        }
    }
}
