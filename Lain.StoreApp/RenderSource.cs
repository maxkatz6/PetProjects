using System;
using Lain.Core;
using Windows.UI.Xaml.Media.Imaging;
using Lain.GAPI;
using Windows.Graphics.Imaging;
using System.IO;
using System.Threading.Tasks;
using Windows.UI.Core;
using SharpDX;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Lain.Graphics;
using Lain.Graphics.GUI;
using Squid;
using Squid.Structs;
using Window = Windows.UI.Xaml.Window;

namespace Lain.StoreApp
{
    public class RenderSource : SurfaceImageSource, IRenderSource
    {
        public bool IsRender { get; set; } = true;
        public bool CountFPS { get; set; } = true;
        public Fps FPS { get; } = new Fps();
        public object Handle => this;

        public void Run(Action render, Action update)
        {
            CompositionTarget.Rendering += (s,e) => 
            {
                update();
                if (!IsRender) return;
                App.Render.BeginDraw();
                render();
                App.Render.EndDraw();
                if (CountFPS) FPS.Frame();
            };
        }
        public RenderSource() : base(Config.Width, Config.Height)
        {
            FileSystem.Current = new StoreAppFileSystem();
            App.Render = Render1.Create(Handle);
            Gui.Renderer = new GuiRenderer();
            Application.Current.Suspending += (s, e) => ((Render1)App.Render).Suspend();

            Window.Current.CoreWindow.KeyDown += (s, a) =>
            {
                Input.KeyDown((Input.Key) a.VirtualKey);
                Gui.SetKeyboard(new[] { new KeyData { Pressed = true, Scancode = (int)a.VirtualKey, Char = (char)a.VirtualKey} });
            };
            Window.Current.CoreWindow.KeyUp += (s, a) =>
            {
                Input.KeyUp((Input.Key) a.VirtualKey);
                Gui.SetKeyboard(new[] { new KeyData { Pressed = false, Scancode = (int)a.VirtualKey } });

            };

            Window.Current.CoreWindow.PointerMoved += (s, a) =>
            {
                Input.SetMouseCoord((int) a.CurrentPoint.Position.X, (int) a.CurrentPoint.Position.Y);
                Gui.SetMouse((int)a.CurrentPoint.Position.X, (int)a.CurrentPoint.Position.Y, a.CurrentPoint.Properties.MouseWheelDelta);
            };
            Window.Current.CoreWindow.PointerPressed += (s, a) => Input.PressRButton(true);
            Window.Current.CoreWindow.PointerReleased += (s, a) => Input.PressRButton(false);
            Window.Current.CoreWindow.PointerCursor = null;
            Texture.FromFileDel = f =>
            {
                try
                {
                    return Task.Run(async () =>
                    {
                        var dec = await BitmapDecoder.CreateAsync(FileSystem.Load(f).AsRandomAccessStream());
                        var pixels = await dec.GetPixelDataAsync(
                            BitmapPixelFormat.Rgba8,
                            BitmapAlphaMode.Straight,
                            new BitmapTransform(),
                            ExifOrientationMode.RespectExifOrientation,
                            ColorManagementMode.DoNotColorManage);
                        unsafe
                        {
                            fixed (void* p = &pixels.DetachPixelData()[0])
                                return Texture.Create(new IntPtr(p), (int)dec.PixelWidth, (int)dec.PixelHeight, SharpDX.DXGI.Format.R8G8B8A8_UNorm);
                        }
                    }).Result;
                }
                catch (Exception ex)
                {
                    Log.SendError("WinRT.LoadTexture() : " + ex.Message + " on load " + f + ".");
                    return Texture.Null;
                }
            };
        }
    }
}