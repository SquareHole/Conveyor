using System.Threading;
using System.Threading.Tasks;
using Talista.Conveyor;

namespace Talista.ConveyorTests.Setup
{
    class TestCommand : ConveyorCommand<TestContext>
    {
        private readonly int _delay;
        private readonly string _runResult;

        public TestCommand(int delay = 0, string runResult = "")
        {
            _delay = delay;
            _runResult = runResult;
        }
        public override async Task Run()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(_delay);
                this.Context.Set("Identifier", this.Context.Identifier);
                this.Context.TestResult += _runResult;
            }, CancellationToken).ConfigureAwait(false);
        }
    }
}
