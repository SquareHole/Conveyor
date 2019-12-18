using Microsoft.Extensions.Logging;

namespace Talista.Conveyor
{
    public interface IConveyorContext
    {
        void Set(string key, object value);
        T Get<T>(string key, bool throwWhenMissing = false);
    }
}