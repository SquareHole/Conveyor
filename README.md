# Conveyor Belt

An attempt at an async Conveyorbelt similar to NChain. This implementation uses System.Threading,Channel to create an Unbounded channel.

![NuGet Generation](https://github.com/Talista/Conveyor/workflows/NuGet%20Generation/badge.svg)

```c#
var channel = Channel.CreateUnbounded<IConveyorCommand<T>>();
```

### ConveyorCommand<T>

`ConveyorCommand<T>` is the implementation of a command that will be executed by as part
of the chain.

`ConveyorCommand<T>` has a `Run()` method that runs Async with a `Task` return type.

#### Command implementation.

```csharp
class TestCommand : ConveyorCommand<TestContext>
{
    private readonly int _delay;
    private readonly string _runResult;

    public TestCommand(int delay = 0, string runResult = "")
    {
        _delay = delay;
        _runResult = runResult;
    }
    public override async Task Run()
    {
        await Task.Delay(_delay, CancellationToken).ContinueWith((a) =>
        {
            CancellationToken.ThrowIfCancellationRequested();
            this.Context.Set("Identifier", this.Context.Identifier);
            this.Context.TestResult.Append(_runResult);
        }, CancellationToken).ConfigureAwait(false);
    }
}
```

### ConveyorContext

`ConveyorContext` is an abstract class that exposes a generic Get/Set command
for contextual data.

```csharp
class TestContext : ConveyorContext
{
    public TestContext(ILogger<TestContext> logger = default) : base(logger)
    {
    }

    public Guid Identifier { get; set; } = Guid.NewGuid();
    public int TestTimeout { get; set; } = 10;

    public StringBuilder TestResult { get; set; } = new StringBuilder();
}
```

### ConveyorBelt<T>

```csharp
class TestConveyor : ConveyorBelt<TestContext>
{
    public TestConveyor(TestContext context, CancellationToken cancellationToken = default)
        : base(context, cancellationToken)
    {
    }
}
```

### Running the Conveyor

```csharp
public async Task RunConveyor()
{
    CancellationTokenSource cts = new CancellationTokenSource(20);

    var context = new TestContext() { TestTimeout = 50 };
    var command = new TestCommand(delay: 150);
    var command2 = new TestCommand(delay: 150);

    var conveyor = new TestConveyor(context, cts.Token);

    await conveyor.Register(command);
    await conveyor.Register(command2);

    await conveyor.Run(true);
}
```

### Attributions

<div>Icons made by <a href="https://www.flaticon.com/authors/xnimrodx" title="xnimrodx">xnimrodx</a> from <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a></div>
