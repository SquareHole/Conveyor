using System.Text;
using Microsoft.Extensions.Logging;
using Talista.Conveyor;

namespace Talista.ConveyorTests.Setup
{
	class TestContext : ConveyorContext
    {
		public TestContext(ILogger<TestContext> logger = default) : base(logger)
		{
		}

        public Guid Identifier { get; set; } = Guid.NewGuid();
        public int TestTimeout { get; set; } = 10;

        public StringBuilder TestResult { get; set; } = new StringBuilder();
    }
}
