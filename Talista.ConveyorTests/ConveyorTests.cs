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
            var command = new TestCommand();
            var conveyor = new TestConveyor(context, cts.Token);

            await conveyor.Register(command);

            await conveyor.Run();
            
            context.Get<Guid>("Identifier").ShouldBe(context.Identifier);
        }
        
        [Test]
        public async Task Conveyor_can_be_cancelled_with_token()
        {
            CancellationTokenSource cts = new CancellationTokenSource(200);

            var context = new TestContext() {TestTimeout = 3000};
            var command = new TestCommand();
            var conveyor = new TestConveyor(context, cts.Token);

            await conveyor.Register(command);

            Should.Throw<TaskCanceledException>(async () => await conveyor.Run());
            
            context.Get<Guid>("Identifier").ShouldBe(context.Identifier);
        }
        
    }
}