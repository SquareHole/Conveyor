using System.Threading;
using System.Threading.Tasks;
using Talista.Conveyor;

namespace Talista.ConveyorTests.Setup
{
    class TestCancellingCommand : ConveyorCommand<TestContext>
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly int _delay;
        private readonly string _runResult;

        public TestCancellingCommand(CancellationTokenSource cancellationTokenSource, int delay = 0,
            string runResult = "") : base()
        {
            _cancellationTokenSource = cancellationTokenSource;
            _delay = delay;
            _runResult = runResult;
        }

        public override async Task Run()
        {
            await Task.Delay(_delay, _cancellationTokenSource.Token).ContinueWith((tc) =>
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                Context.Set("Identifier", Context.Identifier);
                Context.TestResult.Append(_runResult);
            }, CancellationToken);
        }
    }
}
