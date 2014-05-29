using System;
using Ormeli.Core.Patterns;
using Ormeli.Graphics;
using Ormeli.Graphics.Cameras;
using Ormeli.Graphics.Effects;
using Ormeli.Math;
using System.Threading;
using System.Globalization;
using Squid;
using Font = Ormeli.Graphics.Font;
using Point = Squid.Point;
using Rectangle = Squid.Rectangle;

namespace Ormeli
{
    public class Program : Disposable
    {
        /*          TODO необходимое
         * GUI. Font только для одной строчки, чотбы squid работал оптимально
         * ...перекур...
         * Система сцен 
         * Система частиц
         * */

        /*          TODO Возможно
         * Можно заполнить CG types функциями и методами, которые вызывают DllImport функции
         * Добавить HLSL, конвертер в него с CG
         * Поддержка винфона, андроида  
         * ОТДЕЛИТЬ РЕНДЕР ОТ ИНИЦИАЛИЗАЦИИ
         * +-Математическая либа - доделать.
         */

        private static void Main()
        {
            using (var p = new Program())
            {
                Loop.Run(p.Draw, p.Update);
            }
        }

        private readonly Desktop _desktop;
        private readonly Model _plos = new Model();
        private readonly FreeCamera _freeCamera;

        public Program()
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Config.Initialize();
            Config.Fps = 0;
            Config.VerticalSyncEnabled = false;
            Config.FullScreen = false;
            Config.Width = 1020;
            Config.Height = 750;
            Config.Enable4xMSAA = false;

            App.Initialize(Config.IsMono ? RenderType.OpenGl3 : (RenderType)int.Parse(Console.ReadLine()));
            App.Render.BackColor = Color.Indigo;

            var effect = new ColTexEffect("ColTexEffect.cgfx");
            EffectManager.AddEffect(effect);

            var _2DEffect = new Effect2D("2DEffect.cgfx");
            EffectManager.AddEffect(_2DEffect);

            _freeCamera = new FreeCamera( new Vector3( 0,3,0 ), 0, 0, true ){Speed = 1};

            _plos.SetMeshes(GeometryGenerator.CreateGrid(Config.Width, Config.Height));
            
            App.Render.AlphaBlending(true);

            GuiHost.Renderer = new GuiRenderer();

            _desktop = new Desktop {ShowCursor = true, Size = new Point(Config.Width, Config.Height)};
            _desktop.Controls.Add(new ImageControl
            {
                Texture = "NVIDIA.png",
                Size = new Point(128,128),
                Position = new Point(Config.Width - 130, Config.Height - 130)
            });
            _desktop.Controls.Add(new Label
            {
                Text = "Hello, Мир!",
                Position = new Point(500,  500)
            });
		}


        private void Draw()
        {
            App.Render.BeginDraw();

            _plos.Render();
            
            App.Render.EndDraw();
        }

        void Update()
        {
           if (Input.IsKeyDown(Key.Escape)) App.Exit();
        //    _freeCamera.Rotate(0.01f,0);
            if (Input.IsKeyDown(Key.W)) //ловим нажатие и двигаемся
                _freeCamera.DirectionMove(Vector3.Forward);
            else if (Input.IsKeyDown(Key.S))
                _freeCamera.DirectionMove(Vector3.Backward);
            else if (Input.IsKeyDown(Key.A))
                _freeCamera.DirectionMove(Vector3.Left);
            else if (Input.IsKeyDown(Key.D))
                _freeCamera.DirectionMove(Vector3.Right);
            _freeCamera.Update(); // обновляем камеру

   //         _desktop.Update();

            Console.Title = Loop.GetFPS().ToString();
        }
    }
}
