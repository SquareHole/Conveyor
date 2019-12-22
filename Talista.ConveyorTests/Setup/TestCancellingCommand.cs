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

        public TestCancellingCommand(CancellationTokenSource cancellationTokenSource, int delay = 0, string runResult = "")
        {
            _cancellationTokenSource = cancellationTokenSource;
            _delay = delay;
            _runResult = runResult;
        }
        public override async ValueTask Run()
        { 
            await Task.Run(() =>
            {
                _cancellationTokenSource.CancelAfter(_delay);
                Thread.Sleep(_delay * 2);
                this.Context.Set("Identifier", this.Context.Identifier);
                this.Context.TestResult += _runResult;
            }, CancellationToken);
        }
    }
}