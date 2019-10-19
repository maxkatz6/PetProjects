using System;
using System.Diagnostics;
using Lain.Core.Patterns;

namespace Lain.Core
{
    /// <summary>
    ///     Timer helper class
    /// </summary>
    public class Timer : Disposable
    {
        private Stopwatch _stopWatch;

        public Timer(bool autoStart = false)
        {
            if (autoStart) Start();
        }

        /// <summary>
        ///     Gets the frame time.
        /// </summary>
        /// <value>
        ///     The frame time.
        /// </value>
        public long FrameTime { get; private set; }

        /// <summary>
        ///     Runs speed test for some action
        /// </summary>
        /// <param name="act">The Action.</param>
        /// <returns></returns>
        public static long Run(Action act)
        {
            using (var timer = new Timer(true))
            {
                act.BeginInvoke(null, null);
                return timer.Time();
            }
        }

        /// <summary>
        ///     Runs speed test for some action
        /// </summary>
        /// <param name="act">The Action.</param>
        /// <param name="times">How many times repeat action</param>
        /// <param name="returnMid">if set to <c>true</c> [return middle].</param>
        /// <returns></returns>
        public static long Run(Action act, int times, bool returnMid = false)
        {
            using (var timer = new Timer(true))
            {
                for (var i = 0; i < times; i++) act.BeginInvoke(null, null);
                return returnMid ? timer.Time()/times : timer.Time();
            }
        }

        /// <summary>
        ///     Initializes this instance.
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
        ///     Return time
        /// </summary>
        /// <returns>Time in milliseconds</returns>
        public long Time()
        {
            return _stopWatch.ElapsedMilliseconds;
        }

        /// <summary>
        ///     Frames this instance.
        /// </summary>
        public long Frame()
        {
            _stopWatch.Stop();
            FrameTime = _stopWatch.ElapsedMilliseconds;
            _stopWatch.Restart();
            return FrameTime;
        }

        protected override void OnDispose()
        {
            _stopWatch?.Stop();
        }
    }
}