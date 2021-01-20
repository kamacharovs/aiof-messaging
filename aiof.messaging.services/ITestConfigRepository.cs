using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using aiof.messaging.data;

namespace aiof.messaging.services
{
    public interface ITestConfigRepository
    {
        Task<ITestConfig> GetAsync(int id);
    }
}
