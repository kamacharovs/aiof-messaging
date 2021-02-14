using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;

using Newtonsoft.Json;
using AutoMapper;

using aiof.messaging.data;

namespace aiof.messaging.services
{
    public class TableRepository : ITableRepository
    {
        private readonly ILogger<TableRepository> _logger;
        private readonly IMapper _mapper;
        private readonly IEnvConfiguration _envConfig;
        private readonly CloudTableClient _client;

        public TableRepository(
            ILogger<TableRepository> logger,
            IMapper mapper,
            IEnvConfiguration envConfig,
            CloudTableClient client)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _envConfig = envConfig ?? throw new ArgumentNullException(nameof(envConfig));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task LogAsync(
            IMessage message,
            IEmailMessage emailMessage)
        {
            var emailMsgEntity = _mapper.Map<EmailMessageEntity>(emailMessage);

            emailMsgEntity.PartitionKey = _envConfig.EmailQueueName;
            emailMsgEntity.RowKey = message.PublicKey.ToString();

            await InsertOrMergeAsync(_envConfig.EmailTableName, emailMsgEntity);
        }

        public async Task LogAsync(IMessage message)
        {
            var emailEntity = _mapper.Map<MessageEntity>(message);

            await InsertOrMergeAsync(_envConfig.InboundQueueName, emailEntity);
        }

        public async Task<T> InsertOrMergeAsync<T>(
            string tableName,
            T entity) where T : TableEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                var table = _client.GetTableReference(tableName);
                var insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
                var result = await table.ExecuteAsync(insertOrMergeOperation);

                return result.Result as T;
            }
            catch (StorageException e)
            {
                _logger.LogError(e, "Error while inserting {entityName}. Entity={entity}",
                    nameof(T),
                    JsonConvert.SerializeObject(entity));

                throw;
            }
        }
    }
}
