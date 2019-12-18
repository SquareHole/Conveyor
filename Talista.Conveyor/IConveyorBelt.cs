using System.Threading.Tasks;

namespace Talista.Conveyor
{
    public interface IConveyorBelt<T> where T : IConveyorContext
    {
        T Context { get; }
        ValueTask Register(IConveyorCommand<T> command);
        ValueTask Run();
    }
}