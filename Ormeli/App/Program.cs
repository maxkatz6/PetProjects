using System;
using Ormeli.GAPI.Interfaces;
using Ormeli.Graphics.Builders;
using Ormeli.Graphics.Cameras;
using Ormeli.Graphics.Components;
using Ormeli.Graphics.Effects;
using Ormeli.Graphics.Managers;
using Ormeli.Input;
using SharpDX;

namespace Ormeli
{
    public class Program
    {
        /*          TODO необходимое
         * Система сцен 
         * GUI. Font
         * */

        /*          TODO Возможно
         * Можно заполнить CG types функциями и методами, которые вызывают DllImport функции
         * Добавить HLSL, конвертер в него с CG
         * Поддержка винфона, андроида  
         * ОТДЕЛИТЬ РЕНДЕР ОТ ИНИЦИАЛИЗАЦИИ
         */
        private static readonly FreeCamera FreeCamera = new FreeCamera(new Vector3(0,3,0)) {Speed = 10};
        private static readonly Scene Scene = new Scene();

        private static void Main()
        {
            App.Initialize(RenderType.OpenGl3);
            App.Render.BackColor = Color.Indigo;
            
            EffectManager.AddEffect(Effect.LoadFromFile<ColTexEffect>("ColTexEffect.cgfx"));

            Scene.AddObject(ModelBuilder.Create().SetMeshes(GeometryGenerator.CreateGrid(10000, 10000)), new Vector3(0,-2000,0));
            Scene.AddObject(@"Stone Bridge\bridge.obj", Vector3.Zero);

            App.Loop.Run(Redner, Update);
        }

        static void Redner()
        {
            App.Render.BeginDraw();
            
            Scene.Render();

            App.Render.EndDraw();
        }

        static void Update()
        {
            if (Input.Input.IsKeyDown(Key.Escape)) App.Exit();

            if (Input.Input.IsKeyDown(Key.W))
                FreeCamera.DirectionMove(Vector3.ForwardRH);
            else if (Input.Input.IsKeyDown(Key.S))
                FreeCamera.DirectionMove(Vector3.BackwardRH);
            if (Input.Input.IsKeyDown(Key.A))
                FreeCamera.DirectionMove(Vector3.Left);
            else if (Input.Input.IsKeyDown(Key.D))
                FreeCamera.DirectionMove(Vector3.Right);

            Console.Title = App.Loop.FPS.ToString();
        }
    }
}
