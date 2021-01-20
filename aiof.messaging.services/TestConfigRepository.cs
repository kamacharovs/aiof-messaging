using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using aiof.messaging.data;

namespace aiof.messaging.services
{
    public class TestConfigRepository : ITestConfigRepository
    {
        private readonly ILogger<TestConfigRepository> _logger;
        private readonly MessageContext _context;

        public TestConfigRepository(
            ILogger<TestConfigRepository> logger,
            MessageContext context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private IQueryable<TestConfig> GetQuery(bool asNoTracking = true)
        {
            var query = _context.TestConfigs
                .AsQueryable();

            return asNoTracking
                ? query.AsNoTracking()
                : query;
        }

        public async Task<ITestConfig> GetAsync(int id)
        {
            return await GetQuery()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
