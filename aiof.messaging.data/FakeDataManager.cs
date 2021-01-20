using System;
using System.Collections.Generic;
using System.Text;

namespace aiof.messaging.data
{
    public class FakeDataManager
    {
        private readonly MessageContext _context;

        public FakeDataManager(MessageContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void UseFakeContext()
        {
            _context.TestConfigs
                .AddRange(GetFakeTestConfigs());
        }

        public IEnumerable<TestConfig> GetFakeTestConfigs()
        {
            return new List<TestConfig>
            {
                new TestConfig
                {
                    Id = 1,
                    Type = MessageType.Email,
                    Email = "test@test.com",
                    PhoneNumber = null
                }
            };
        }
    }
}
