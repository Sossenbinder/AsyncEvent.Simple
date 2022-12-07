using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncEvent
{
	internal class AsyncEvent<TEventArgs> : IAsyncEvent<TEventArgs>
	{
		private readonly ConcurrentDictionary<Func<TEventArgs, Task>, object> _handlers;

		public AsyncEvent()
			: this(new ConcurrentDictionary<Func<TEventArgs, Task>, object>())
		{
		}

		public AsyncEvent(ConcurrentDictionary<Func<TEventArgs, Task>, object> handlers)
		{
			_handlers = handlers;
		}

		public IDisposable Register(Func<TEventArgs, Task> handler)
		{
			_handlers.TryAdd(handler, null);
			return new DisposableEventHandler(() => Unregister(handler));
		}

		public void Unregister(Func<TEventArgs, Task> handler)
		{
			_handlers.TryRemove(handler, out _);
		}

		public async Task Raise(TEventArgs args)
		{
			if (_handlers.Count == 0)
			{
				return;
			}

			await Task.WhenAll(_handlers.Select(x => x.Key.Invoke(args))).ConfigureAwait(false);
		}
	}
}