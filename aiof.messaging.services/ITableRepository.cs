using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using aiof.messaging.data;

namespace aiof.messaging.services
{
    public interface ITableRepository
    {
        Task<MessageEntity> InsertOrMergeAsync(
            string tableName,
            MessageEntity messageEntity);
    }
}
