using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Talista.Conveyor;

namespace Talista.ConveyorTests.Setup
{
    class TestCancellingCommand : ConveyorCommand<TestContext>
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly int _delay;
        private readonly string _runResult;
        private readonly ILogger _logger;

        public TestCancellingCommand(CancellationTokenSource cancellationTokenSource, int delay = 0,
	        string runResult = "", ILogger logger = null)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _delay = delay;
            _runResult = runResult;
            _logger = logger;
        }

        public override async Task Run()
        {
	        _logger?.LogDebug("Staring command {command}", nameof(TestCancellingCommand));
	        await Task.Delay(_delay, _cancellationTokenSource.Token).ContinueWith(tc =>
	        {
		        _cancellationTokenSource.Cancel();
		        _cancellationTokenSource.Token.ThrowIfCancellationRequested();
		        Context.Set("Identifier", Context.Identifier);
		        Context.TestResult.Append(_runResult);
	        }, CancellationToken);

	        _logger?.LogDebug("Completed command {command}", nameof(TestCancellingCommand));
        }
    }
}
