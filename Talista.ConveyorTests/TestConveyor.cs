using System.Threading;
using Talista.Conveyor;

namespace Talista.ConveyorTests
{
    class TestConveyor : ConveyorBelt<TestContext>
    {
        public TestConveyor(TestContext context, CancellationToken cancellationToken = default) 
            : base(context, cancellationToken)
        {
        }
        
    }
}