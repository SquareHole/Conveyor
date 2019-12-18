using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Talista.Conveyor.Interfaces;

namespace Talista.Conveyor
{
    public abstract class ConveyorCommand<T> : IConveyorCommand<T> where T : IConveyorContext
    {
        private ILogger<ConveyorCommand<T>> _logger;

        protected ConveyorCommand()
        {
            _logger = LoggerFactory.Create(b => b.AddConsole())
                .CreateLogger<ConveyorCommand<T>>();
        }

        public T Context { get; set; }

        public CancellationToken CancellationToken { get; set; }

        public abstract ValueTask Run();

    }
}