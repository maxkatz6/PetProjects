using System;
using Ormeli.Core;

namespace Ormeli.Graphics
{
	public class Loop
	{
		private const int MaxFrameSkip = 10;
		private readonly Fps _fps = new Fps();
		private readonly Timer _timer = new Timer();
		private uint _frameDuration;

		public Loop()
		{
			IsRender = true;
			CountFPS = true;
		}

		public bool IsRender { get; set; }
		public bool CountFPS { get; set; }
		public int FPS => _fps.Value;

		public void Run(Action render, Action update)
		{
			if (Config.VerticalSyncEnabled || Config.Fps == 0)
			{
				App.Render.Run(() =>
				{
					update();
					if (!IsRender) return;
					App.Render.BeginDraw();
					render();
					App.Render.EndDraw();
					if (CountFPS) _fps.Frame();
				});
			}
			else
			{
				_timer.Start();

				_frameDuration = 1000/Config.Fps;

				var nextFrameTime = _timer.Time();

				int loops;
				App.Render.Run(() =>
				{
					loops = 0;
					while (_timer.Time() > nextFrameTime && loops < MaxFrameSkip)
					{
						update();

						if (IsRender)
						{
							App.Render.BeginDraw();
							render();
							App.Render.EndDraw();
							if (CountFPS) _fps.Frame();
						}

						nextFrameTime += _frameDuration;

						loops++;
					}
				});
			}
		}
	}
}