using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncEvent
{
	public class AsyncEvent : IAsyncEvent
	{
		private readonly ConcurrentDictionary<Func<Task>, object> _handlers;

		public AsyncEvent()
			: this(new ConcurrentDictionary<Func<Task>, object>())
		{
		}

		public AsyncEvent(ConcurrentDictionary<Func<Task>, object> handlers)
		{
			_handlers = handlers;
		}

		public IDisposable Register(Func<Task> handler)
		{
			_handlers.TryAdd(handler, null);
			return new DisposableEventHandler(() => Unregister(handler));
		}

		public void Unregister(Func<Task> handler)
		{
			_handlers.TryRemove(handler, out _);
		}

		public async Task Raise()
		{
			if (_handlers.Count == 0)
			{
				return;
			}

			await Task.WhenAll(_handlers.Select(x => x.Key.Invoke())).ConfigureAwait(false);
		}
	}
}