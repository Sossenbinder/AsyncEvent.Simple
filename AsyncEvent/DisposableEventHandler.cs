using System;

namespace AsyncEvent
{
	internal class DisposableEventHandler : IDisposable
	{
		private readonly Action _disposalCallback;

		public DisposableEventHandler(Action disposalCallback) => _disposalCallback = disposalCallback;

		public void Dispose() => _disposalCallback();
	}
}