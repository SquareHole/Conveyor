using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Talista.Conveyor;

namespace Talista.ConveyorTests.Setup
{
    class TestCommand : ConveyorCommand<TestContext>
    {
        private readonly int _delay;
        private readonly string _runResult;
        private readonly ILogger _logger;

        public TestCommand(int delay = 0, string runResult = "", ILogger logger = null)
        {
            _delay = delay;
            _runResult = runResult;
            _logger = logger;
        }
        public override async Task Run()
        {
	        _logger?.LogDebug("Command started: {command}", nameof(TestCommand));
            await Task.Delay(_delay, CancellationToken).ContinueWith(a =>
            {
                CancellationToken.ThrowIfCancellationRequested();
                Context.Set("Identifier", Context.Identifier);
                Context.TestResult.Append(_runResult);
            }, CancellationToken).ConfigureAwait(false);

            _logger?.LogDebug("Completed command : {command}", nameof(TestCommand));
        }
    }
}
