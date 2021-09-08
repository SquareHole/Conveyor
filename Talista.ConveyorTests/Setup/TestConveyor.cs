using Microsoft.Extensions.Logging;
using Talista.Conveyor;

namespace Talista.ConveyorTests.Setup
{
	class TestConveyor : ConveyorBelt<TestContext>
    {
	    public TestConveyor(TestContext context, ILogger logger, CancellationToken cancellationToken = default)
            : base(context, logger, cancellationToken)
        {
        }

    }
}
