using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos.Table;

using aiof.messaging.data;

namespace aiof.messaging.services
{
    public interface ITableRepository
    {
        Task LogAsync(
            IMessage message,
            IEmailMessage emailMessage);

        Task LogAsync(IMessage message);

        Task<T> InsertOrMergeAsync<T>(
            string tableName,
            T entity) where T : TableEntity;
    }
}
