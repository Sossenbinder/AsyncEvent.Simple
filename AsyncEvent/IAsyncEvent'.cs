using System;
using System.Threading.Tasks;

namespace AsyncEvent;

public interface IAsyncEvent
{
	IDisposable Register(Func<Task> handler);

	void Unregister(Func<Task> handler);

	Task Raise();
}

public interface IAsyncEvent<TEventArgs>
{
	IDisposable Register(Func<TEventArgs, Task> handler);

	void Unregister(Func<TEventArgs, Task> handler);

	Task Raise(TEventArgs args);
}