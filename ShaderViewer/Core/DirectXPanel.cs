using System.Windows.Forms;
using SharpDX;
using SharpDX.Windows;
using TDF.Core;
using TDF.Graphics.Render;
using TDF.Inputs;

namespace TDFExample_ModelConverter.Core
{
    /// <summary>
    ///
    /// </summary>
    public class  DirectXPanel : RenderControl, IShutdown
    {
        public RenderLoop.RenderCallback Loop;

        public void Run()
        {
            Resize += (sender, args) =>
            {
                Config.Height = (sender as Control).Height;
                Config.Width = (sender as Control).Width;
                Config.CurrentCamera.Projection = Matrix.PerspectiveFovLH(MathHelper.ToRadians(45), (float)Config.Width / Config.Height, Config.ScreenNear, Config.ScreenDepth);
                Config.CurrentCamera.Ortho = Matrix.OrthoLH(Config.Width, Config.Height, Config.ScreenNear, Config.ScreenDepth);
                DirectX11.Resize();
            };
            Show();
            RenderLoop.Run(this, Loop);
        }

        public void Shutdown()
        {
            Loop = null;
            Dispose();
        }


        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0102: //WM_CHAR
                    {
                        switch ((int)m.WParam)
                        {
                            case 0x0A: // linefeed
                            case 13:
                                break;

                            case 0x08: // backspace
                                //Input.CharInput((char)0x08);
                                break;

                            case 0x1B: // escape
                                break;

                            case 0x09: // tab
                              //  for (int i = 0; i < 4; i++)
                                   // Input.CharInput((char)m.WParam);
                                break;

                            default: // displayable character
                               // Input.CharInput((char)m.WParam);
                                break;
                        }
                        break;
                    }
                case 0x20b://(int)WinMesage.XBUTTONDOWN:
                    {
                        switch ((int)m.WParam)
                        {
                            case 0x20040://верхняя
                                {
                                    this.Text = "up";
                                    break;
                                }
                            case 0x10020://нижняя
                                {
                                    Text = "down";
                                    break;
                                }
                        }
                        break;
                    }
                case 0x0201: //WM_LMouseDown
                    {
                        Input.LeftButton(true);
                        Input.KeyDown(Key.Lbutton);
                        break;
                    }
                case 0x0202: //WM_LMouseUp
                    {
                        Input.LeftButton(false);
                        Input.KeyUp(Key.Lbutton);
                        break;
                    }
                case 0x0204: //WM_RMouseDown
                    {
                        Input.RightButton(true);
                        Input.KeyDown(Key.Rbutton);
                        break;
                    }
                case 0x0205: //WM_RMouseUp
                    {
                        Input.RightButton(false);
                        Input.KeyUp(Key.Rbutton);
                        break;
                    }
                case 0x0207: //WM_MMouseDown
                    {
                        Input.MiddleButton(true);
                        Input.KeyDown(Key.Mbutton);
                        break;
                    }
                case 0x0208: //WM_MMouseUp
                    {
                        Input.MiddleButton(false);
                        Input.KeyUp(Key.Mbutton);
                        break;
                    }
                case 0x0100://Wm_KeyDown
                    {
                        Input.KeyDown((Key)m.WParam);
                        break;
                    }
                case 0x0101:
                    {
                        Input.KeyUp((Key)m.WParam);
                        break;
                    }
                case 0x0006://WinMesage.ACTIVATE:
                    {
                        if ((int)m.WParam == 0)
                        {
                        }
                        else
                        {
                            if (Config.FullScreen) DirectX11.UpdateScreenMode();
                        }
                        break;
                    }
            }

            base.WndProc(ref m);
        }
    }
}