using System.Collections.Generic;
using System.Linq;
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
        private readonly ILogger _logger;
        private  bool _running;

        protected ConveyorBelt(T context, ILogger logger, CancellationToken cancellationToken = default)
        {
	        _cancellationToken = cancellationToken;
            Context = context;
            context.Token = cancellationToken;
            var channel = Channel.CreateUnbounded<IConveyorCommand<T>>();
            _reader = channel.Reader;
            _writer = channel.Writer;
            _logger = logger;
        }

        public T Context { get; }

        public async Task Register(IConveyorCommand<T> command)
        {
            command.Context = Context;
            command.CancellationToken = _cancellationToken;
            await _writer.WriteAsync(command, _cancellationToken).ConfigureAwait(false);
        }

        public async Task Run(bool parallel = false)
        {
            if (parallel)
            {
                await RunInParallel();
            }
            else
            {
                await RunSequentially();
            }
        }

        private async ValueTask RunSequentially()
        {
            if (_running)
            {
                //The conveyor belt is already running, log and exit
                _logger.LogInformation($"Run was called on a running conveyor : {GetType().FullName}");
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

        private async Task RunInParallel()
        {
            if (_running)
            {
                //The conveyor belt is already running, log and exit
                _logger.LogInformation($"Run was called on a running conveyor : {GetType().FullName}");
                return;
            }

            var commands = new List<Task>();

            _logger.LogDebug("Conveyor started running.");
            _running = true;
            _writer.Complete();
            while (await _reader.WaitToReadAsync(_cancellationToken))
            {
                var command = await _reader.ReadAsync(_cancellationToken);
                _cancellationToken.ThrowIfCancellationRequested();
                commands.Add(command.Run());
            }

            var tasks = commands.Select(t => t).ToArray();
             Task.WaitAll(tasks, _cancellationToken);

            _running = false;
        }
    }
}
