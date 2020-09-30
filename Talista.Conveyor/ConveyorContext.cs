using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Talista.Conveyor.Interfaces;

namespace Talista.Conveyor
{
	public abstract class ConveyorContext : IConveyorContext
    {
        private readonly ILogger<ConveyorContext> _logger;
        private readonly Dictionary<string, object> _contextData;
        protected ConveyorContext(ILogger<ConveyorContext> logger = default)
        {
            _logger = logger ?? LoggerFactory.Create(b => b.AddConsole())
                .CreateLogger<ConveyorContext>();
            _contextData = new Dictionary<string, object>();
        }
        public void Set(string key, object value) => _contextData.TryAdd(key, value);

        public T Get<T>(string key, bool throwWhenMissing = false)
        {
            if (_contextData.TryGetValue(key, out var value))
            {
                return (T)value;
            }

            if (throwWhenMissing)
            {
	            _logger.LogError("Could not find value for key : {key}", key);
                throw new MissingExpectedContextDataException(key);
            }
            return default;
        }

        public CancellationToken Token { get; set; }
    }
}
