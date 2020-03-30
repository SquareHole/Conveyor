# Conveyor Belt
An attempt at a Conveyorbelt similar to NChain. This iomplementation uses System.Threading,Channel to create an Unbounded channel.

```c#
var channel = Channel.CreateUnbounded<IConveyorCommand<T>>();
```
### IConveyorCommand<T>

The generic type is an instance of an `IConveyorContext`.

`IConveyorCommand<T>` has a `Run()` method that runs Async with a `Task` return type.
