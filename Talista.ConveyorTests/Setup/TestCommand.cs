using System.Threading;
using System.Threading.Tasks;
using Talista.Conveyor;

namespace Talista.ConveyorTests.Setup
{
    class TestCommand : ConveyorCommand<TestContext>
    {
        private readonly int _delay;
        private readonly string _runResult;

        public TestCommand(int delay = 0, string runResult = "") : base()
        {
            _delay = delay;
            _runResult = runResult;
        }
        public override async Task Run()
        {
            await Task.Delay(_delay, CancellationToken).ContinueWith((a) =>
            {
                CancellationToken.ThrowIfCancellationRequested();
                Context.Set("Identifier", Context.Identifier);
                Context.TestResult.Append(_runResult);
            }, CancellationToken).ConfigureAwait(false);
        }
    }
}
