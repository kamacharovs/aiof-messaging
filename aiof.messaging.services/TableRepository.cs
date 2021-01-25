using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;

using Newtonsoft.Json;

using aiof.messaging.data;

namespace aiof.messaging.services
{
    public class TableRepository : ITableRepository
    {
        private readonly ILogger<TableRepository> _logger;
        private readonly CloudTableClient _client;

        public TableRepository(
            ILogger<TableRepository> logger,
            CloudTableClient client)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<MessageEntity> InsertOrMergeAsync(
            string tableName,
            MessageEntity messageEntity)
        {
            if (messageEntity == null)
            {
                throw new ArgumentNullException(nameof(messageEntity));
            }

            try
            {
                var table = _client.GetTableReference(tableName);
                var insertOrMergeOperation = TableOperation.InsertOrMerge(messageEntity);
                var result = await table.ExecuteAsync(insertOrMergeOperation);

                return result.Result as MessageEntity;
            }
            catch (StorageException e)
            {
                _logger.LogError(e, "Error while inserting {entityName}. Entity={entity}",
                    nameof(MessageEntity),
                    JsonConvert.SerializeObject(messageEntity));

                throw;
            }
        }
    }
}
