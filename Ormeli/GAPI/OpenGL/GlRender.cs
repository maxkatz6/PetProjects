using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Ormeli.Core.Patterns;
using Ormeli.GAPI.Interfaces;
using Ormeli.Graphics.Cameras;
using Ormeli.Input;
using SharpDX;
using Buffer = Ormeli.Graphics.Components.Buffer;
using MouseButtons = OpenTK.Input.MouseButton;


namespace Ormeli.GAPI.OpenGL
{
    internal sealed class GlRender : Disposable, IRender
    {
        private GameWindow _gameWindow;

        public Color4 BackColor
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                GL.ClearColor(_color.Red, _color.Green, _color.Blue, _color.Alpha);
            }
        }

        private Color4 _color;

        public void CreateWindow()
        {
            _gameWindow = new GameWindow(Config.Width, Config.Height, new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(32), 24, 8, Config.Enable4xMSAA ? 4 : 1), Config.Tittle, Config.FullScreen ? GameWindowFlags.Fullscreen : GameWindowFlags.Default)
            {
                VSync = Config.VerticalSyncEnabled ? VSyncMode.On : VSyncMode.Off
            };
            _gameWindow.Resize += (sender, args) =>
            {
                Camera.Current.UpdateScrennMatrices(_gameWindow.Width, _gameWindow.Height);
                GL.Viewport(_gameWindow.ClientRectangle);
            };
            _gameWindow.KeyPress += (s, e) => Input.Input.CharInput(e.KeyChar);
            _gameWindow.KeyDown += (s, e) => Input.Input.KeyDown(e.Key.Convert());
            _gameWindow.KeyUp += (s, e) => Input.Input.KeyUp(e.Key.Convert());
            _gameWindow.MouseMove += (s, e) => Input.Input.SetMouseCoord(e.X, e.Y);
            _gameWindow.MouseDown += (s, e) =>
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        Input.Input.LeftButton(true);
                        break;
                    case MouseButtons.Right:
                        Input.Input.RightButton(true);
                        break;
                    case MouseButtons.Middle:
                        Input.Input.MiddleButton(true);
                        break;
                }
            };
            _gameWindow.MouseUp += (s, e) =>
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        Input.Input.LeftButton(false);
                        break;
                    case MouseButtons.Right:
                        Input.Input.RightButton(false);
                        break;
                    case MouseButtons.Middle:
                        Input.Input.MiddleButton(false);
                        break;
                }
            };
        }

        public RenderType Initialize()
        {
            GL.Color4(0, 0, 0, 0f);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
            HardwareDescription.VideoCardDescription = GL.GetString(StringName.Vendor).Split(' ')[0] +
                                                       GL.GetString(StringName.Renderer).Split('/')[0];
            HardwareDescription.VideoCardMemory = 0;

            if (App.EffectLanguage == EffectLanguage.CG)
                Cg.CgEffectBase.InitOpenGl();
            else throw new Exception("OpenGLInit. Unsupport shader type");

            return RenderType.OpenGl3;
        }

        public ICreator GetCreator()
        {
            return new GlCreator();
        }


        public void Run(Action act)
        {
            _gameWindow.RenderFrame += (sender, args) => act();
            _gameWindow.Run();
        }

        public void BeginDraw()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit );
        }




        public void EndDraw()
        {
            _gameWindow.SwapBuffers();
        }

        private bool _t, _oldAlpha, _oldZ;

        public bool ZBuffer(bool turn)
        {
            if (turn == _oldZ) return _oldZ;
            if (turn) GL.Enable(EnableCap.DepthTest);
            else GL.Disable(EnableCap.DepthTest);
            _t = _oldZ;
            _oldZ = turn;
            return _t;
        }

        public bool AlphaBlending(bool turn)
        {
            if (turn == _oldAlpha) return _oldAlpha;
            if (turn) GL.Enable(EnableCap.Blend);
            else GL.Disable(EnableCap.Blend);
            _t = _oldAlpha;
            _oldAlpha = turn;
            return _t;
        }

        private static int _lastBuffer;
        public void SetBuffers(Buffer vertexBuffer, Buffer indexBuffer, int vertexStride)
        {
            if (_lastBuffer == vertexBuffer)
                return;
            _lastBuffer = vertexBuffer;
            GL.BindVertexArray(vertexBuffer);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
        }

        public void Draw(int indexCount)
        {
            GL.DrawElements(PrimitiveType.Triangles, indexCount,
                DrawElementsType.UnsignedInt, IntPtr.Zero);
        }
        public void Draw(int indexCount, int startIndex)
        {
            GL.DrawRangeElements(PrimitiveType.TriangleStrip, startIndex, startIndex+indexCount, indexCount,
                DrawElementsType.UnsignedInt, IntPtr.Zero);
        }
    }

    public static class Helper
    {
        public static Key Convert(this OpenTK.Input.Key k)
        {
            switch (k)
            {
                case OpenTK.Input.Key.Q:
                    return Key.Q;
                case OpenTK.Input.Key.W:
                    return Key.W;
                case OpenTK.Input.Key.E:
                    return Key.E;
                case OpenTK.Input.Key.R:
                    return Key.R;
                case OpenTK.Input.Key.T:
                    return Key.T;
                case OpenTK.Input.Key.Y:
                    return Key.Y;
                case OpenTK.Input.Key.U:
                    return Key.U;
                case OpenTK.Input.Key.I:
                    return Key.I;
                case OpenTK.Input.Key.O:
                    return Key.O;
                case OpenTK.Input.Key.P:
                    return Key.P;
                case OpenTK.Input.Key.A:
                    return Key.A;
                case OpenTK.Input.Key.S:
                    return Key.S;
                case OpenTK.Input.Key.D:
                    return Key.D;
                case OpenTK.Input.Key.F:
                    return Key.F;
                case OpenTK.Input.Key.G:
                    return Key.G;
                case OpenTK.Input.Key.H:
                    return Key.H;
                case OpenTK.Input.Key.J:
                    return Key.J;
                case OpenTK.Input.Key.K:
                    return Key.K;
                case OpenTK.Input.Key.L:
                    return Key.L;
                case OpenTK.Input.Key.Z:
                    return Key.Z;
                case OpenTK.Input.Key.X:
                    return Key.X;
                case OpenTK.Input.Key.C:
                    return Key.C;
                case OpenTK.Input.Key.V:
                    return Key.V;
                case OpenTK.Input.Key.B:
                    return Key.B;
                case OpenTK.Input.Key.N:
                    return Key.N;
                case OpenTK.Input.Key.M:
                    return Key.M;

                case OpenTK.Input.Key.F1:
                    return Key.F1;
                case OpenTK.Input.Key.F2:
                    return Key.F2;
                case OpenTK.Input.Key.F3:
                    return Key.F3;
                case OpenTK.Input.Key.F4:
                    return Key.F4;
                case OpenTK.Input.Key.F5:
                    return Key.F5;
                case OpenTK.Input.Key.F6:
                    return Key.F6;
                case OpenTK.Input.Key.F7:
                    return Key.F7;
                case OpenTK.Input.Key.F8:
                    return Key.F8;
                case OpenTK.Input.Key.F9:
                    return Key.F9;
                case OpenTK.Input.Key.F10:
                    return Key.F10;
                case OpenTK.Input.Key.F11:
                    return Key.F11;
                case OpenTK.Input.Key.F12:
                    return Key.F12;

                case OpenTK.Input.Key.AltLeft:
                case OpenTK.Input.Key.AltRight:
                    return Key.Alt;

                case OpenTK.Input.Key.ShiftLeft:
                case OpenTK.Input.Key.ShiftRight:
                    return Key.Shift;

                case OpenTK.Input.Key.BackSpace:
                    return Key.Back;

                case OpenTK.Input.Key.ControlLeft:
                case OpenTK.Input.Key.ControlRight:
                    return Key.Control;


                case OpenTK.Input.Key.WinLeft:
                    return Key.Winleft;
                case OpenTK.Input.Key.WinRight:
                    return Key.Winright;
                case OpenTK.Input.Key.Menu:
                    return Key.Menu;


                case OpenTK.Input.Key.Up:
                    return Key.Up;
                case OpenTK.Input.Key.Down:
                    return Key.Down;
                case OpenTK.Input.Key.Left:
                    return Key.Left;
                case OpenTK.Input.Key.Right:
                    return Key.Right;


                case OpenTK.Input.Key.KeypadEnter:
                    return Key.Enter;
                case OpenTK.Input.Key.Enter:
                    return Key.Enter;
                case OpenTK.Input.Key.Escape:
                    return Key.Escape;
                case OpenTK.Input.Key.Space:
                    return Key.Space;
                case OpenTK.Input.Key.Tab:
                    return Key.Tab;
                case OpenTK.Input.Key.Insert:
                    return Key.Insert;
                case OpenTK.Input.Key.Delete:
                    return Key.Delete;
                case OpenTK.Input.Key.PageUp:
                    return Key.Pageup;
                case OpenTK.Input.Key.PageDown:
                    return Key.Pagedown;
                case OpenTK.Input.Key.Home:
                    return Key.Home;
                case OpenTK.Input.Key.End:
                    return Key.End;

                case OpenTK.Input.Key.CapsLock:
                    return Key.CapsLock;
                case OpenTK.Input.Key.ScrollLock:
                    return Key.Scroll;
                case OpenTK.Input.Key.PrintScreen:
                    return Key.PrintScreen;
                case OpenTK.Input.Key.Pause:
                    return Key.Pause;
                case OpenTK.Input.Key.NumLock:
                    return Key.NumLock;



                case OpenTK.Input.Key.Clear:
                    return Key.Clear;


                case OpenTK.Input.Key.Keypad0:
                    return Key.D0;
                case OpenTK.Input.Key.Keypad1:
                    return Key.D1;
                case OpenTK.Input.Key.Keypad2:
                    return Key.D2;
                case OpenTK.Input.Key.Keypad3:
                    return Key.D3;
                case OpenTK.Input.Key.Keypad4:
                    return Key.D4;
                case OpenTK.Input.Key.Keypad5:
                    return Key.D5;
                case OpenTK.Input.Key.Keypad6:
                    return Key.D6;
                case OpenTK.Input.Key.Keypad7:
                    return Key.D7;
                case OpenTK.Input.Key.Keypad8:
                    return Key.D8;
                case OpenTK.Input.Key.Keypad9:
                    return Key.D9;


                case OpenTK.Input.Key.Number0:
                    return Key.N0;
                case OpenTK.Input.Key.Number1:
                    return Key.N1;
                case OpenTK.Input.Key.Number2:
                    return Key.N2;
                case OpenTK.Input.Key.Number3:
                    return Key.N3;
                case OpenTK.Input.Key.Number4:
                    return Key.N4;
                case OpenTK.Input.Key.Number5:
                    return Key.N5;
                case OpenTK.Input.Key.Number6:
                    return Key.N6;
                case OpenTK.Input.Key.Number7:
                    return Key.N7;
                case OpenTK.Input.Key.Number8:
                    return Key.N8;
                case OpenTK.Input.Key.Number9:
                    return Key.N9;


                case OpenTK.Input.Key.Plus:
                    return Key.Plus;
                case OpenTK.Input.Key.KeypadAdd:
                    return Key.Add;
                case OpenTK.Input.Key.Minus:
                    return Key.Subtract;
                case OpenTK.Input.Key.KeypadMinus:
                    return Key.Minus;
                case OpenTK.Input.Key.KeypadMultiply:
                    return Key.Multiply;
                case OpenTK.Input.Key.KeypadDivide:
                    return Key.Divide;

                case OpenTK.Input.Key.Semicolon:
                    return Key.Oem1;
                case OpenTK.Input.Key.Tilde:
                    return Key.Oem3;
                case OpenTK.Input.Key.BracketLeft:
                    return Key.Oem4;
                case OpenTK.Input.Key.BracketRight:
                    return Key.Oem6;
                case OpenTK.Input.Key.Quote:
                    return Key.Oem7;

                case OpenTK.Input.Key.Comma:
                    return Key.Comma;
                case OpenTK.Input.Key.Period:
                    return Key.Period;
                case OpenTK.Input.Key.KeypadDecimal:
                    return Key.Decimal;
                case OpenTK.Input.Key.Slash:
                    return (Key) 191;
                case OpenTK.Input.Key.BackSlash:
                    return Key.BackSlash;

                default:
                    return (Key) (int) k;
            }
        }
    }
}