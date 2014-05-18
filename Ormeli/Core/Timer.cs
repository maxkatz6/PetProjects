using System;
using System.Diagnostics;
using Ormeli.Core.Patterns;

namespace Ormeli.Core
{
    /// <summary>
    /// Timer helper class
    /// </summary>
    public class Timer : Disposable
    {
        private Stopwatch _stopWatch;

        /// <summary>
        /// Gets the frame time.
        /// </summary>
        /// <value>
        /// The frame time.
        /// </value>
        public long FrameTime { get; private set; }

        /// <summary>
        /// Runs speed test for some action
        /// </summary>
        /// <param name="act">The Action.</param>
        /// <returns></returns>
        public static long Run(Action act)
        {
            var timer = new Timer();
            timer.Start();
            act.BeginInvoke(null, null);
            timer._stopWatch.Stop();
            return timer.Time();
        }

        /// <summary>
        /// Runs speed test for some action
        /// </summary>
        /// <param name="act">The Action.</param>
        /// <param name="times">How many times repeat action</param>
        /// <param name="returnMid">if set to <c>true</c> [return middle].</param>
        /// <returns></returns>
        public static long Run(Action act, int times, bool returnMid = false)
        {
            var timer = new Timer();
            timer.Start();
            for (int i = 0; i < times; i++) act.BeginInvoke(null, null);
            timer._stopWatch.Stop();
            return returnMid ? timer.Time() / times : timer.Time();
        }

        /// <summary>
        /// Frames this instance.
        /// </summary>
        public void Frame()
        {
            _stopWatch.Stop();

            FrameTime = _stopWatch.ElapsedMilliseconds;

            _stopWatch.Restart();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            // Check to see if this system supports high performance timers.
            if (Stopwatch.Frequency == 0)
                return false;

            _stopWatch = Stopwatch.StartNew();

            return true;
        }

        /// <summary>
        /// Return time
        /// </summary>
        /// <returns>Time in milliseconds</returns>
        public long Time()
        {
          return _stopWatch.ElapsedMilliseconds;
        }

        protected override void OnDispose()
        {
            _stopWatch.Stop();
            _stopWatch = null;
        }
    }
}