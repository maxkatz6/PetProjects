using System;

namespace Ormeli.Core
{
	/// <summary>
	/// FramesPerSecond counter
	/// </summary>
	public class Fps
	{
		private int _count;
		private TimeSpan _startTime;

		public Fps()
		{
			Value = 0;
			_count = 0;
			_startTime = DateTime.Now.TimeOfDay;
		}

		public int Value { get; set; }

		public void Frame()
		{
			_count++;

			var delta = (DateTime.Now.TimeOfDay - _startTime).Seconds;
			if (delta < 1) return;
			Value = _count;
			_count = 0;

			_startTime = DateTime.Now.TimeOfDay;
		}
	}
}