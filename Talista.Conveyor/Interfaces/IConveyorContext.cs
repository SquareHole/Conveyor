namespace Talista.Conveyor.Interfaces
{
	public interface IConveyorContext
    {
        void Set(string key, object value);
        T Get<T>(string key, bool throwWhenMissing = false);

        CancellationToken Token { get; set; }
    }
}
