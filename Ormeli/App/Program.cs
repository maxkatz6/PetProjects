using System;
using Ormeli.Core;
using Ormeli.Core.Patterns;
using Ormeli.DirectX11;
using Ormeli.Graphics;
using Ormeli.Graphics.Effects;
using Ormeli.Math;
using Ormeli.OpenGL;

namespace Ormeli
{
    public class Program : Disposable
    {
        /*          TODO необходимое
         * исправить баги
         * Каждый Мэш должен знать свой эффект и нужную технику  
         * ОТДЕЛИТЬ РЕНДЕР ОТ ИНИЦИАЛИЗАЦИИ
         * Инпут
         * Работа с камерами (копи паст из ЕВЫ)
         * Доработать цикл. ФПС
         * Рендер 3D, повороты моделей
         * Система сцен 
         * 2D графика.
         * Сравнение с ЕВОЙ
         * Использование дополнительный библиотек для запуска OpenGL версии под линуксом
         * 
         * Развитие дальше. Поздравляю, ты догнал ЕВУ, надеюсь, будет быстрее */

        /*          TODO Возможно
         * Можно заполнить CG types функциями и методами, которые вызывают DllImport функции
         * Добавить HLSL, конвертер в него с CG
         * Поддержка винфона, андроида  */

        /*          TODO Problems
         * Загрузка текстуры в шейдер в DirectX11 (что за фигня?) */

        /*          TODO Срок - 1 июля. Если до этого момента я не закончу необходимое, то бросаю проект как неудавшийся  */

        static void Main(string[] args)
        {
            var t = new Timer();
            t.Start();
            var pr =  new Program();
            t.Frame();
            Console.WriteLine(t.FrameTime);
            t.Dispose();
            App.Render.Run(pr.Draw);
        }

        private readonly Mesh<ColorVertex> mesh = new Mesh<ColorVertex>();

        public Program()
        {
            Config.Height = 500;
            Config.Width = 500;

            App.Initialize(Console.ReadLine() == "1" ? (IRender) new DxRender() : new GlRender());
            App.Render.BackColor = Color.Indigo;


            var effect = new ColTexEffect("effect.cgfx");
            effect.SetTexture(App.Creator.LoadTexture("NVIDIA.png"));
            EffectManager.Effects.Add(effect);

                    mesh.Initalize(new[] { 0, 1, 2}, new[]
                     {
                         new ColorVertex(new Vector3(-1, 1,0), Color.Wheat),
                         new ColorVertex(new Vector3( 1, 1, 0), Color.Yellow),
                         new ColorVertex(new Vector3( 0, -1, 0), Color.AliceBlue)
                     });
         /*   mesh.Initalize(new[] { 0, 1, 2, 0, 2, 3 }, new[]
            {
                new TextureVertex(new Vector3(-0.8f,  -0.8f,0), new Vector2(0,0)),
                new TextureVertex(new Vector3( -0.8f,  0.8f, 0),new Vector2(1,0)),
                new TextureVertex(new Vector3(0.8f, 0.8f, 0), new Vector2(1,1)),
                new TextureVertex(new Vector3(0.8f,  -0.8f,0), new Vector2(0,1)),
            });*/
        }

        private void Draw()
        {
            App.Render.BeginDraw();
            mesh.Render();
            App.Render.EndDraw();
        }
    }
}
