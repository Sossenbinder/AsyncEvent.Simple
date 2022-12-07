namespace AsyncEvent.Test
{
	public class Tests
	{
		private IAsyncEvent _event = default!;

		[SetUp]
		public void SetUp()
		{
			_event = new AsyncEvent();
		}

		[Test]
		public void EventShouldWorkForZeroHandlers()
		{
			Assert.DoesNotThrowAsync(_event.Raise);
		}

		[Test]
		public async Task EventShouldHandleHandlersCorrectly()
		{
			var handled = false;

			Task DoHandle()
			{
				handled = true;
				return Task.CompletedTask;
			}

			_event.Register(DoHandle);
			await _event.Raise();

			Assert.That(handled);
		}

		[Test]
		public async Task EventShouldProperlyUnregisterHandler()
		{
			var handled = false;

			Task DoHandle()
			{
				handled = true;
				return Task.CompletedTask;
			}

			_event.Register(DoHandle);
			_event.Unregister(DoHandle);

			await _event.Raise();

			Assert.That(!handled);
		}

		[Test]
		public async Task EventShouldProperlyUnregisterHandlerThroughDispose()
		{
			var handled = false;

			Task DoHandle()
			{
				handled = true;
				return Task.CompletedTask;
			}

			using (_event.Register(DoHandle)) { }

			await _event.Raise();

			Assert.That(!handled);
		}
	}
}