using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Talista.Conveyor.Interfaces;

namespace Talista.Conveyor
{
    public abstract class ConveyorBelt<T> : IConveyorBelt<T> where T : IConveyorContext
    {
        private readonly CancellationToken _cancellationToken;
        private readonly ChannelReader<IConveyorCommand<T>> _reader;
        private readonly ChannelWriter<IConveyorCommand<T>> _writer;
        private readonly ILogger<ConveyorBelt<T>> _logger;
        private  bool _running;

        protected ConveyorBelt(T context, CancellationToken cancellationToken = default)
        {
            _cancellationToken = cancellationToken;
            Context = context;
            var channel = Channel.CreateUnbounded<IConveyorCommand<T>>();
            _reader = channel.Reader;
            _writer = channel.Writer;
            _logger = LoggerFactory.Create(b => b.AddConsole())
                .CreateLogger<ConveyorBelt<T>>();
        }

        public T Context { get; }

        public async ValueTask Register(IConveyorCommand<T> command)
        {
            command.Context = this.Context;
            await _writer.WriteAsync(command, _cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask Run()
        {
            if (_running)
            {
                //The conveyor belt is already running, log and exit
                _logger.LogInformation($"Run was called on a running conveyor : {this.GetType().FullName}");
                return;
            }

            _logger.LogDebug("Conveyor started running.");
            _running = true;
            _writer.Complete();
            while (await _reader.WaitToReadAsync(_cancellationToken))
            {
                var command = await _reader.ReadAsync(_cancellationToken);
                await command.Run();
            }

            _running = false;
        }
    }
}