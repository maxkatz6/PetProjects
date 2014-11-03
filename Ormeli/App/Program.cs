using System;
using Ormeli.Graphics;
using Ormeli.Graphics.Cameras;
using Ormeli.Graphics.Effects;
using SharpDX;

namespace Ormeli
{
    public class Program
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
        private static readonly Model Plos = new Model();
        private static readonly FreeCamera FreeCamera = new FreeCamera(new Vector3(0,3,0)) {Speed = 10};

        private static void Main()
        {
            App.Initialize(RenderType.DirectX11);
            App.Render.BackColor = Color.Indigo;

            EffectManager.AddEffect(Effect.LoadFromFile<ColTexEffect>("ColTexEffect.cgfx"));

            Plos.SetMeshes(GeometryGenerator.CreateGrid(1200, 1200));

            App.Run(Draw, Update);
        }

        static void Draw()
        {
            App.Render.BeginDraw();
            
            Plos.Render();

            App.Render.EndDraw();
        }

        static void Update()
        {
            if (Input.IsKeyDown(Key.Escape)) App.Exit();

            if (Input.IsKeyDown(Key.W))
                FreeCamera.DirectionMove(Vector3.ForwardRH);
            else if (Input.IsKeyDown(Key.S))
                FreeCamera.DirectionMove(Vector3.BackwardRH);
            if (Input.IsKeyDown(Key.A))
                FreeCamera.DirectionMove(Vector3.Left);
            else if (Input.IsKeyDown(Key.D))
                FreeCamera.DirectionMove(Vector3.Right);

            FreeCamera.Update(); // обновляем камеру

            Console.Title = App.Loop.FPS.ToString();
        }
    }
}
