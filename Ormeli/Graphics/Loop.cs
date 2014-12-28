﻿using System;
using Ormeli.Core;
using Ormeli.Graphics.Cameras;

namespace Ormeli.Graphics
{
    public class Loop
    {
        public bool IsRender { get; set; }
        public bool CountFPS { get; set; }

        private readonly Timer _timer = new Timer();
        private const int MaxFrameSkip = 10;
        private uint _frameDuration;
        private readonly Fps _fps = new Fps();

        public uint FPS => (uint)_fps.Value;

        public Loop()
        {
            IsRender = true;
            CountFPS = true;
        }

        public void Run(Action render, Action update)
        {
            
            if (Config.VerticalSyncEnabled || Config.Fps == 0)
            {
                App.Render.Run(() =>
                {
                    update();
                    if (!IsRender) return;
                    render();
                    if (CountFPS) _fps.Frame();
                });
            }
            else
            {
                _timer.Start();

                _frameDuration = 1000 / Config.Fps;

                var nextFrameTime = _timer.Time();

                int loops;
                App.Render.Run(() =>
                {
                    loops = 0;
                    while (_timer.Time() > nextFrameTime && loops < MaxFrameSkip)
                    {
                        update();
                        Camera.Current.Update();

                        if (IsRender)
                        {
                            render();
                            if (CountFPS) _fps.Frame();
                        }

                        nextFrameTime += _frameDuration;

                        loops++;
                    }
                });
            }
        }
        //http://habrahabr.ru/post/150640/
        public void Run(Action render, Action<double> update)
        {
            _timer.Start();
            
            var currentFrameTime = _timer.Time();

            App.Render.Run(() =>
            {
                var prevFrameTime = currentFrameTime;
                currentFrameTime = _timer.Time();

                // сколько миллискунд прошло между прошлой и текущей итерацией:
                var dt = currentFrameTime - prevFrameTime;

                // поделим на 1000 т.к. с секундами мысленно работать проще,
                // чем с миллисекундами.
                update((double)dt / 1000);
                Camera.Current.Update();

                if (!IsRender) return;
                render();
                if (CountFPS) _fps.Frame();
            });
        }
    }
}
