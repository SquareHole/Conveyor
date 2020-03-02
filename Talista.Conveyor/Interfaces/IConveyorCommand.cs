using System.Threading;
using System.Threading.Tasks;

namespace Talista.Conveyor.Interfaces
{
    public interface IConveyorCommand<T> where T : IConveyorContext
    {
        T Context { get; set; }

        CancellationToken CancellationToken { get; set; }
        Task Run();
    }
}
