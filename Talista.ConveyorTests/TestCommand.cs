using System.Threading.Tasks;
using Talista.Conveyor;

namespace Talista.ConveyorTests
{
    class TestCommand : ConveyorCommand<TestContext>
    {
        public TestCommand() : base()
        {
        }

        public override async ValueTask Run()
        { 
            await Task.Run(() =>
            {
                System.Threading.Thread.Sleep(this.Context.TestTimeout);
                this.Context.Set("Identifier", this.Context.Identifier);
            });
        }
    }
}