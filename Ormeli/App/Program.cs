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
         * Поддержка винфона, андроида  
         * ОТДЕЛИТЬ РЕНДЕР ОТ ИНИЦИАЛИЗАЦИИ
         * /

        /*          TODO Problems
         * Загрузка текстуры в шейдер в DirectX11 (что за фигня?) */

        /*          TODO Срок - 1 июля. Если до этого момента я не закончу необходимое, то бросаю проект как неудавшийся  */

        private static void Main(string[] args)
        {
            using (var p = new Program())
            {
                App.Render.Run(p.Draw);
            }
        }

        private readonly Mesh<TextureVertex> mesh = new Mesh<TextureVertex>();

        public Program()
        {
            Config.Height = 500;
            Config.Width = 500;

            App.Initialize(Console.ReadLine() == "1" ? (IRender) new DxRender() : new GlRender());
            App.Render.BackColor = Color.Indigo;


            var effect = new ColTexEffect("effect.cgfx");
            effect.SetTexture(App.Creator.LoadTexture("NVIDIA.png"));
            EffectManager.Effects.Add(effect);

            /*mesh.Initalize(0, new[] {0, 1, 2, 0, 2, 3}, new[]
            {
                new ColorVertex(new Vector3(-0.8f, -0.8f, 0), Color.Red),
                new ColorVertex(new Vector3(-0.8f, 0.8f, 0), Color.White),
                new ColorVertex(new Vector3(0.8f, 0.8f, 0), Color.Blue),
                new ColorVertex(new Vector3(0.8f, -0.8f, 0), Color.Green),
            });*/
               mesh.Initalize(0,new[] { 0, 1, 2, 0, 2, 3 }, new[]
            {
                new TextureVertex(new Vector3(-0.8f,  -0.8f,0), new Vector2(0,0)),
                new TextureVertex(new Vector3( -0.8f,  0.8f, 0),new Vector2(1,0)),
                new TextureVertex(new Vector3(0.8f, 0.8f, 0), new Vector2(1,1)),
                new TextureVertex(new Vector3(0.8f,  -0.8f,0), new Vector2(0,1)),
            });
        }

        private void Draw()
        {
            App.Render.BeginDraw();
            mesh.Render();
            App.Render.EndDraw();
        }
    }
}
