using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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

        public abstract ValueTask Run();

    }
}