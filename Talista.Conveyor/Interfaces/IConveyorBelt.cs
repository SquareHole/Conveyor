namespace Talista.Conveyor.Interfaces
{
	public interface IConveyorBelt<T> where T : IConveyorContext
    {
        T Context { get; }
        Task Register(IConveyorCommand<T> command);
        Task Run(bool parallel = false);
    }
}
