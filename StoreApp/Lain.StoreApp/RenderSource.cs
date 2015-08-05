using System;
using Lain.Core;
using Windows.UI.Xaml.Media.Imaging;
using Lain.GAPI;
using Windows.Graphics.Imaging;
using System.IO;
using System.Threading.Tasks;
using SharpDX;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Lain.Graphics;

namespace Lain.StoreApp
{
    public class RenderSource : SurfaceImageSource, IRenderSource
    {
        private readonly Fps _fps = new Fps();
        public bool IsRender { get; set; } = true;
        public bool CountFPS { get; set; } = true;
        public int FPS => _fps.Value;
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
                if (CountFPS) _fps.Frame();
            };
        }

        public RenderSource() : base(Config.Width, Config.Height)
        {
            FileSystem.Current = new StoreAppFileSystem();
            App.Render = Render1.Create(Handle);
            Application.Current.Suspending += (s, e) => ((Render1)App.Render).Suspend();
            Window.Current.CoreWindow.KeyDown += (s, a) => Input.KeyDown((Input.Key)a.VirtualKey);
            Window.Current.CoreWindow.KeyUp += (s, a) => Input.KeyUp((Input.Key)a.VirtualKey);
            Window.Current.CoreWindow.PointerMoved += (s, a) => Input.SetMouseCoord((int)a.CurrentPoint.Position.X, (int)a.CurrentPoint.Position.Y);
            Window.Current.CoreWindow.PointerPressed += (s, a) => Input.PressRButton(true);
            Window.Current.CoreWindow.PointerReleased += (s, a) => Input.PressRButton(false);
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
                    ErrorProvider.SendError("WinRT.LoadTexture() : " + ex.Message + " on load " + f + ".");
                    return Texture.Null;
                }
            };
        }
    }
}