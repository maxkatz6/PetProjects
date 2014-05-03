#region Using

using System;
using System.Security.Permissions;
using System.Windows.Forms;
using SharpDX.Windows;
using TDF.Core;
using TDF.Graphics.Render;
using TDF.Inputs;

#endregion

namespace TDFExample_ModelConverter.Core
{
    /// <summary>
    /// </summary>
    public class DirectXPanel : RenderControl, IShutdown
    {
        public Action Draw;
        public new Action Update;

        private readonly Fps _fps = new Fps();

        public int FPS { get { return _fps.Value; } }

        public void Shutdown()
        {
            Dispose();
        }

        public void Run(Action u, Action d)
        {
            Update = u;
            Draw = d;

            Resize += (sender, args) =>
            {
                var c = ((Control) sender);
                Config.Height = c.Height;
                Config.Width = c.Width;
                DirectX11.Resize();
            };
            Show();
            RenderLoop.Run(this, Render);
        }

        private void Render()
        {
            Update();

            DirectX11.BeginScene(BackColor.ToColor());
            _fps.Frame();
            Draw();
            DirectX11.EndScene();
        }


        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0201: //WM_LMouseDown
                    Input.LeftButton(true);
                    Input.KeyDown(Key.Lbutton);
                    break;
                case 0x0202: //WM_LMouseUp
                    Input.LeftButton(false);
                    Input.KeyUp(Key.Lbutton);
                    break;
                case 0x0204: //WM_RMouseDown
                    Input.RightButton(true);
                    Input.KeyDown(Key.Rbutton);
                    break;
                case 0x0205: //WM_RMouseUp
                    Input.RightButton(false);
                    Input.KeyUp(Key.Rbutton);
                    break;
                case 0x0207: //WM_MMouseDown
                    Input.MiddleButton(true);
                    Input.KeyDown(Key.Mbutton);
                    break;
                case 0x0208: //WM_MMouseUp
                    Input.MiddleButton(false);
                    Input.KeyUp(Key.Mbutton);
                    break;
                case 0x0100: //Wm_KeyDown
                    Input.KeyDown((Key) m.WParam);
                    break;
                case 0x0101:
                    Input.KeyUp((Key) m.WParam);
                    break;
            }
            base.WndProc(ref m);
        }
    }
}