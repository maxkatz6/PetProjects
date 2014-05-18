using System;
using Ormeli.Core.Patterns;
using Ormeli.Graphics;
using Ormeli.Graphics.Cameras;
using Ormeli.Graphics.Effects;
using Ormeli.Math;

namespace Ormeli
{
    public class Program : Disposable
    {
        /*          TODO необходимое
         * 2D графика.
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

        private static void Main(string[] args)
        {
            using (var p = new Program())
            {
                Loop.Run(p.Draw, p.Update);
            }
        }

        private readonly Model _model = new Model();
        private readonly Model _plos = new Model();
        private readonly FreeCamera _freeCamera;

        public Program()
        {
            Config.Initialize();
            Config.Fps = 0;
            Config.VerticalSyncEnabled = false;
            Config.FullScreen = false;
            Config.Width = 1920;
            Config.Height = 1080;
            Config.Enable4xMSAA = true;

            App.Initialize((RenderType)int.Parse(Console.ReadLine()));
            App.Render.BackColor = Color.Indigo;
            
            var effect = new ColTexEffect( "ColTexEffect.cgfx" );
            EffectManager.AddEffect( effect );

            _freeCamera = new FreeCamera( new Vector3( 0,3,0 ), 0, 0, true ){Speed = 10};

            _plos.SetMeshes(GeometryGenerator.CreateGrid(100, 100), CreateColorGrid(1000, 1000));
            
            _model.SetMeshes(GeometryGenerator.CreateBox(20, 20,20));
            _model.GetMesh<TextureMesh>(0).TextureNum = Texture.GetNumber("NVIDIA.png");

            App.Render.ZBuffer(true);
        }
        private void Draw()
        {
            App.Render.BeginDraw();

            _plos.Render();

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 15; j++)
                    for (int k = 0; k < 10; k++)
                    {
                        _model.SetWorldMatrix(Matrix.Translation(21*i, 21*j + 500, 21*k));
                        _model.Render();
                    }

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

            Console.Title = Loop.GetFPS().ToString();
        }


        private static ColorMesh CreateColorGrid(int width, int depth)
        {
            var w2 = 0.5f * width;
            var d2 = 0.5f * depth;
            var mV = new[]
            {
                new ColorVertex(new Vector3(-w2,-1,-d2), Color.Aqua), 
                new ColorVertex(new Vector3(w2,-1,-d2), Color.Aqua),
                new ColorVertex(new Vector3(w2,-1,d2), Color.Indigo),
                new ColorVertex(new Vector3(-w2,-1,d2), Color.Indigo)
            };
            var m = new ColorMesh();
            m.Initialize(new[] { 0, 1, 2, 0, 2, 3 }, mV);
            return m;
        }
    }
}
