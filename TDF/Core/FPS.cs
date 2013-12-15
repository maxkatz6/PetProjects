using System;

namespace TDF.Core
{
    /// <summary>
    /// FramesPerSecond f counter
    /// </summary>
    public class Fps
    {
        private int _count;

        private TimeSpan _startTime;

        public int Value { get; private set; }

        /// <summary>
        /// Frames this instance.
        /// </summary>
        public void Frame()
        {
            _count++;

            var delta = (DateTime.Now.TimeOfDay - _startTime).Seconds;
            if (delta < 1) return;
            Value = _count;
            _count = 0;

            _startTime = DateTime.Now.TimeOfDay;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            Value = 0;
            _count = 0;
            _startTime = DateTime.Now.TimeOfDay;
        }
    }
}