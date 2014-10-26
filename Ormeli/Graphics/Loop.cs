using System;
using Ormeli.Core;

namespace Ormeli.Graphics
{
    public static class Loop
    {
        public static bool IsRender { get; set; }
        public static bool CountFPS { get; set; }

        private static readonly Timer Timer = new Timer();
        private const int MaxFrameSkip = 10;
        private static uint _frameDuration;
        private static readonly Fps FPS = new Fps(); 

        static Loop()
        {
            IsRender = true;
            CountFPS = true;
        }

        public static void Run(Action render, Action update)
        {
            
            if (Config.VerticalSyncEnabled || Config.Fps == 0)
            {
                App.Render.Run(() =>
                {
                    update();
                    if (!IsRender) return;
                    render();
                    if (CountFPS) FPS.Frame();
                });
            }
            else
            {
                Timer.Start();

                _frameDuration = 1000 / Config.Fps;

                long nextFrameTime = Timer.Time();

                int loops;
                App.Render.Run(() =>
                {
                    loops = 0;
                    while (Timer.Time() > nextFrameTime && loops < MaxFrameSkip)
                    {
                        update();

                        if (IsRender)
                        {
                            render();
                            if (CountFPS) FPS.Frame();
                        }

                        nextFrameTime += _frameDuration;

                        loops++;
                    }
                });
            }
        }
        //http://habrahabr.ru/post/150640/
        public static void Run(Action render, Action<double> update)
        {
            Timer.Start();
            
            long currentFrameTime = Timer.Time();

            App.Render.Run(() =>
            {
                long prevFrameTime = currentFrameTime;
                currentFrameTime = Timer.Time();

                // сколько миллискунд прошло между прошлой и текущей итерацией:
                long dt = currentFrameTime - prevFrameTime;

                // поделим на 1000 т.к. с секундами мысленно работать проще,
                // чем с миллисекундами.
                update((double)dt / 1000);
                if (!IsRender) return;
                render();
                if (CountFPS) FPS.Frame();
            });
        }

        public static uint GetFPS()
        {
            return (uint)FPS.Value;
        }
    }
}
