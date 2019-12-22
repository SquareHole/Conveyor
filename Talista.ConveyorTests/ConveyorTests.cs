using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using Talista.Conveyor;
using Talista.ConveyorTests.Setup;
using TestContext = Talista.ConveyorTests.Setup.TestContext;

namespace Talista.ConveyorTests
{
    public class ConveyorTests
    {
        [Test]
        public void CanCreateContextInstance()
        {
            var context = new TestContext();
            context.Set("test","test");
            
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
            CancellationTokenSource cts = new CancellationTokenSource();
            
            var context = new TestContext();
            var conveyor = new TestConveyor(context, cts.Token);

            await conveyor.Register(new TestCommand(runResult: "A"));
            await conveyor.Register(new TestCommand(runResult: "B"));
            await conveyor.Register(new TestCommand(runResult: "C"));

            await conveyor.Run();
            
            context.Get<Guid>("Identifier").ShouldBe(context.Identifier);
            context.TestResult.ShouldBe("ABC");
        }
        
        [Test]
        public async Task Can_Execute_InParallel()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            
            var context = new TestContext();
            var conveyor = new TestConveyor(context, cts.Token);

            await conveyor.Register(new TestCommand(runResult: "A", delay: 20));
            await conveyor.Register(new TestCommand(runResult: "B", delay: 15));
            await conveyor.Register(new TestCommand(runResult: "C", delay: 10));

            await conveyor.Run(true);
            
            context.Get<Guid>("Identifier").ShouldBe(context.Identifier);
            context.TestResult.ShouldBe("CBA");
        }
        
        [Test]
        public async Task Command_can_cancel_parallel_execution()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            
            var context = new TestContext();
            var conveyor = new TestConveyor(context, cts.Token);

            await conveyor.Register(new TestCommand(runResult: "A", delay: 200));
            await conveyor.Register(new TestCancellingCommand(cts, runResult: "B", delay: 150));
            await conveyor.Register(new TestCommand(runResult: "C", delay: 100));
            
            Should.Throw<TaskCanceledException>(async () => await conveyor.Run(true));
            conveyor.Context.TestResult.ShouldBe("C");
            
        }

        [Test]
        public async Task Command_can_cancel_sequential_execution()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            
            var context = new TestContext();
            var conveyor = new TestConveyor(context, cts.Token);

            await conveyor.Register(new TestCommand(runResult: "A", delay: 50));
            await conveyor.Register(new TestCancellingCommand(cts, runResult: "B", delay: 30));
            await conveyor.Register(new TestCommand(runResult: "C", delay: 10));
            
            Should.Throw<TaskCanceledException>(async () => await conveyor.Run(false));
        }

        
        [Test]
        public async Task Conveyor_can_be_cancelled_with_token()
        {
            CancellationTokenSource cts = new CancellationTokenSource(20);

            var context = new TestContext() {TestTimeout = 10};
            var command = new TestCommand(delay: 50);
            var conveyor = new TestConveyor(context, cts.Token);

            await conveyor.Register(command);

            Should.Throw<TaskCanceledException>(async () => await conveyor.Run());
        }
    }
}