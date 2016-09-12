using System;
using System.Windows.Forms;
using Lain.Core;
using SharpDX.Windows;
using Timer = Lain.Core.Timer;
using Lain.Graphics;
using Lain.GAPI;
using Lain.Graphics.GUI;
using Squid.Structs;

namespace Lain.Desktop
{
	public class RenderSource : IRenderSource
    {
		private const int MaxFrameSkip = 10;
		private readonly Timer _timer = new Timer();
		private uint _frameDuration;
		public bool CountFPS { get; set; } = true;
	    public Fps FPS { get; } = new Fps();
	    private RenderForm renderForm = new RenderForm(Config.Tittle);
        public object Handle => renderForm.Handle;

        public void Run(Action render, Action update)
		{
			if (Config.VerticalSyncEnabled || Config.Fps == 0)
			{
				RenderLoop.Run(renderForm, () =>
				{
					update();
					App.Render.BeginDraw();
					render();
					App.Render.EndDraw();
					if (CountFPS) FPS.Frame();
				});
			}
			else
			{
				_timer.Start();

				_frameDuration = 1000/Config.Fps;

				var nextFrameTime = _timer.Time();

				int loops;
				RenderLoop.Run(renderForm, () =>
				{
					loops = 0;
					while (_timer.Time() > nextFrameTime && loops < MaxFrameSkip)
					{
						update();
                        
						App.Render.BeginDraw();
						render();
						App.Render.EndDraw();
						if (CountFPS) FPS.Frame();

						nextFrameTime += _frameDuration;

						loops++;
					}
				});
			}
		}

		public RenderSource() 
		{
            FileSystem.Current = new DesktopFileSystem();
            App.Render = Render.Create(Handle);
            DevIL.Initialize();

            Squid.Gui.Renderer = new GuiRenderer();

            renderForm.Cursor.Dispose();
            renderForm.Height = Config.Height;
            renderForm.Width = Config.Width;
            renderForm.WindowState = Config.FullScreen ? FormWindowState.Maximized : FormWindowState.Normal;
            renderForm.StartPosition = FormStartPosition.CenterScreen;
            
            renderForm.MouseWheel += (s,e) => Squid.Gui.SetMouse(e.X, e.Y, e.Delta);
            renderForm.KeyPress += (s, e) => { Input.CharInput(e.KeyChar); Squid.Gui.SetKeyboard(new [] { new KeyData { Pressed = true, Char = e.KeyChar } }); };
            renderForm.KeyDown += (s, e) => { Input.KeyDown((Input.Key)e.KeyValue); Squid.Gui.SetKeyboard(new [] { new KeyData {  Pressed = true, Scancode = e.KeyValue} }); };
            renderForm.KeyUp += (s, e) => { Input.KeyUp((Input.Key)e.KeyValue); Squid.Gui.SetKeyboard(new [] { new KeyData { Released = true, Scancode = e.KeyValue } }); };
            renderForm.MouseMove += (s, e) => { Input.SetMouseCoord(e.X, e.Y); Squid.Gui.SetMouse(e.X, e.Y, e.Delta); };
            renderForm.MouseDown += (s, e) =>
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
            renderForm.MouseUp += (s, e) =>
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
		}
	}
}