using System.Threading.Tasks;

namespace Talista.Conveyor.Interfaces
{
    public interface IConveyorCommand<T> where T : IConveyorContext
    {
        T Context { get; set; }
        ValueTask Run();
    }
}