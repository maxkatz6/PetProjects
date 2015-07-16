#if DX
using System;
using System.Windows.Forms;
using Ormeli.Core;
using SharpDX.Windows;
using Timer = Ormeli.Core.Timer;

namespace Ormeli.GAPI
{
	public class Window
	{
		private const int MaxFrameSkip = 10;
		private readonly Fps _fps = new Fps();
		private readonly Timer _timer = new Timer();
		private uint _frameDuration;
		internal RenderForm RenderForm { get; set; }
		public bool IsRender { get; set; } = true;
		public bool CountFPS { get; set; } = true;
		public int FPS => _fps.Value;
		public IntPtr Handle => RenderForm.Handle;

		public void Run(Action render, Action update)
		{
			if (Config.VerticalSyncEnabled || Config.Fps == 0)
			{
				RenderLoop.Run(RenderForm, () =>
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
				RenderLoop.Run(RenderForm, () =>
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

		public static Window Create()
		{
			var window = new Window();
			window.RenderForm = new RenderForm(Config.Tittle)
			{
				Height = Config.Height,
				Width = Config.Width
			};
			window.RenderForm.KeyPress += (s, e) => Input.CharInput(e.KeyChar);
			window.RenderForm.KeyDown += (s, e) => Input.KeyDown((Input.Key) e.KeyValue);
			window.RenderForm.KeyUp += (s, e) => Input.KeyUp((Input.Key) e.KeyValue);
			window.RenderForm.MouseMove += (s, e) => Input.SetMouseCoord(e.X, e.Y);
			window.RenderForm.MouseDown += (s, e) =>
			{
				switch (e.Button)
				{
					case MouseButtons.Left:
						Input.PressLButton(true);
						break;
					case MouseButtons.Right:
						Input.PressRButton(true);
						break;
					case MouseButtons.Middle:
						Input.PressMButton(true);
						break;
				}
			};
			window.RenderForm.MouseUp += (s, e) =>
			{
				switch (e.Button)
				{
					case MouseButtons.Left:
						Input.PressLButton(false);
						break;
					case MouseButtons.Right:
						Input.PressRButton(false);
						break;
					case MouseButtons.Middle:
						Input.PressMButton(false);
						break;
				}
			};
			return window;
		}
	}
}
#endif