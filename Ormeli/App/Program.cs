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
         * Фиксинг
         * Мелочи
         * GUI. Font только для одной строчки, чотбы squid работал оптимально
         * Система сцен 
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

        private readonly Model _plos = new Model();
        private readonly FreeCamera _freeCamera;

        public Program()
		{
            App.Initialize(RenderType.DirectX11);
            App.Render.BackColor = Color.Indigo;

            EffectManager.AddEffect(Effect.LoadFromFile<ColTexEffect>("ColTexEffect.cgfx"));

            _freeCamera = new FreeCamera( new Vector3( 0,3,0 ), 0, 0, true ){Speed = 10};

            _plos.SetMeshes(GeometryGenerator.CreateGrid(1200, 1200));
		}

        void Draw()
        {
            App.Render.BeginDraw();
            
            _plos.Render();

            App.Render.EndDraw();
        }

        void Update()
        {
            if (Input.IsKeyDown(Key.Escape)) App.Exit();

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
    }
}
