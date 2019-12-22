using System.Threading.Tasks;

namespace Talista.Conveyor.Interfaces
{
    public interface IConveyorBelt<T> where T : IConveyorContext
    {
        T Context { get; }
        ValueTask Register(IConveyorCommand<T> command);
        ValueTask Run(bool parallel = false);
    }
}