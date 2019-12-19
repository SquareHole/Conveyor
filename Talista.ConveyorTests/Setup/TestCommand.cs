using System.Threading.Tasks;
using Talista.Conveyor;

namespace Talista.ConveyorTests.Setup
{
    class TestCommand : ConveyorCommand<TestContext>
    {
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