using Talista.Conveyor.Interfaces;

namespace Talista.Conveyor
{
	public abstract class ConveyorCommand<T> : IConveyorCommand<T> where T : IConveyorContext
    {
	    public T Context { get; set; }

        public CancellationToken CancellationToken { get; set; }

        public abstract Task Run();

    }
}
