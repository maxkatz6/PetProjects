using System;

namespace Ormeli.Core.Patterns
{
	public abstract class Disposable : IDisposable
	{
		private bool _disposed;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~Disposable()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed) return;
			if (disposing)
			{
				OnDispose();
			}
			_disposed = true;
		}

		protected virtual void OnDispose()
		{
		}
	}
}