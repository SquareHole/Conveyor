using System.Threading;
using System.Threading.Tasks;

namespace Talista.Conveyor
{
    public interface IConveyorCommand<T> where T : IConveyorContext
    {
        T Context { get; set; }
        ValueTask Run();
    }
}