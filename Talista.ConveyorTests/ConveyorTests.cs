using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Shouldly;
using Talista.Conveyor;
using Talista.ConveyorTests.Setup;
using TestContext = Talista.ConveyorTests.Setup.TestContext;

namespace Talista.ConveyorTests
{
    public class ConveyorTests
    {
	    private ILoggerFactory _loggerFactory;

	    [SetUp]
	    public void SetUp() => _loggerFactory = LoggerFactory.Create(config =>
	    {
		    config.SetMinimumLevel(LogLevel.Trace);
		    config.AddConsole();
	    });

	    [Test]
        public void CanCreateContextInstance()
        {
            var context = new TestContext();
            context.Set("test", "test");

            context.ShouldNotBeNull();
            context.Get<string>("test").ShouldBe("test");
            context.Get<int>("doesnotexist").ShouldBe(0);
        }

        [Test]
        public void Missing_context_key_raises_exception()
        {
            var context = new TestContext();
            Should.Throw<MissingExpectedContextDataException>(() => context.Get<string>("test", true));
        }

        [Test]
        public async Task Can_execute_conveyor_belt()
        {
            var cts = new CancellationTokenSource();
            var logger = _loggerFactory.CreateLogger(typeof(TestConveyor));

            var context = new TestContext();
            var conveyor = new TestConveyor(context, logger,  cts.Token);

            await conveyor.Register(new TestCommand(runResult: "A", logger: logger));
            await conveyor.Register(new TestCommand(runResult: "B", logger: logger));
            await conveyor.Register(new TestCommand(runResult: "C", logger: logger));

            await conveyor.Run();

            context.Get<Guid>("Identifier").ShouldBe(context.Identifier);
            context.TestResult.ToString().ShouldBe("ABC");
        }

        [Test]
        public async Task Can_Execute_InParallel()
        {
            var cts = new CancellationTokenSource();
            var logger = _loggerFactory.CreateLogger(typeof(TestConveyor));

            var context = new TestContext();
            var conveyor = new TestConveyor(context, logger, cts.Token);

            await conveyor.Register(new TestCommand(runResult: "A", delay: 660, logger: logger));
            await conveyor.Register(new TestCommand(runResult: "B", delay: 435, logger: logger));
            await conveyor.Register(new TestCommand(runResult: "C", delay: 110, logger: logger));

            await conveyor.Run(true);

            //context.Get<Guid>("Identifier").ShouldBe(context.Identifier);
            context.TestResult.ToString().ShouldBe("CBA");
        }

        [Test]
        public async Task Command_can_cancel_parallel_execution()
        {
            var cts = new CancellationTokenSource();
            var logger = _loggerFactory.CreateLogger(typeof(TestConveyor));

            var context = new TestContext();
            var conveyor = new TestConveyor(context, logger, cts.Token);

            await conveyor.Register(new TestCommand(runResult: "A", delay: 200, logger: logger));
            await conveyor.Register(new TestCancellingCommand(cts, runResult: "B", delay: 150, logger: logger));
            await conveyor.Register(new TestCommand(runResult: "C", delay: 100, logger: logger));

            Should.Throw<TaskCanceledException>(async () => await conveyor.Run(true));
            conveyor.Context.TestResult.ToString().ShouldBe("C");

        }

        [Test]
        public async Task Command_can_cancel_sequential_execution()
        {
            var cts = new CancellationTokenSource();
            var logger = _loggerFactory.CreateLogger(typeof(TestConveyor));

            var context = new TestContext();
            var conveyor = new TestConveyor(context, logger, cts.Token);

            await conveyor.Register(new TestCommand(runResult: "A", delay: 50, logger: logger));
            await conveyor.Register(new TestCancellingCommand(cts, runResult: "B", delay: 30, logger: logger));
            await conveyor.Register(new TestCommand(runResult: "C", delay: 10, logger: logger));

            Should.Throw<TaskCanceledException>(async () => await conveyor.Run());
        }


        [Test]
        public async Task Conveyor_can_be_cancelled_with_token()
        {
            var cts = new CancellationTokenSource(20);
            var logger = _loggerFactory.CreateLogger(typeof(TestConveyor));

            var context = new TestContext { TestTimeout = 50 };
            var command = new TestCommand(150);
            var command2 = new TestCommand(150);
            var conveyor = new TestConveyor(context, logger, cts.Token);

            await conveyor.Register(command);
            await conveyor.Register(command2);

            Should.Throw<TaskCanceledException>(async () => await conveyor.Run(true));
        }
    }
}
