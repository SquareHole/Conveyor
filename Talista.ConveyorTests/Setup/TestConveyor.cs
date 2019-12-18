using System.Threading;
using Talista.Conveyor;

namespace Talista.ConveyorTests.Setup
{
    class TestConveyor : ConveyorBelt<TestContext>
    {
        public TestConveyor(TestContext context, CancellationToken cancellationToken = default) 
            : base(context, cancellationToken)
        {
        }
        
    }
}