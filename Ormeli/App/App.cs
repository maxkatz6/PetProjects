using System;
using Ormeli.Core;
using Ormeli.GAPI;
using Ormeli.Graphics;

namespace Ormeli
{
	public static class App
	{
		/*          TODO необходимое
		 * Just DO IT! Keep It Simple, Stupid!
		 * Доработка системы сцен. http://docs.unity3d.ru/ScriptReference/Transform.html http://www.gamedev.ru/code/forum/?id=184194 http://www.gamedev.ru/pages/warzes/forum/?id=182048&page=4
		 * GUI. Font. Signed Distance Field http://habrahabr.ru/post/215905/ http://www.gamedev.ru/code/forum/?id=175763&page=2
		 * Текст в геометрическом шейдере http://blogs.agi.com/insight3d/index.php/2008/08/29/rendering-text-fast/ 
		 *          TODO Возможно
		 * Поддержка DirectX12. Изначально как миррорная версия или отдельное GAPI
		 * Постобработка
		 * Частицы
		 * JSON как контейнер информации
		 * Скриптинг
         */
		internal static Render Render;
		internal static Creator Creator;
		internal static readonly Loop Loop = new Loop();

		public static void Main()
		{
			using (var t = new Timer(true))
			{
				Creator.InitGAPI(out Render, out Creator);

				Console.WriteLine(Config.Render + " renderer started in " + t.Frame() + " microseconds");

				var Scene = new Scene();
				Scene.Load();

				Console.WriteLine("Scene loaded in " + t.Frame() + " microseconds");

				Scene.Run();
			}
		}

		public static void Exit(int code = 0)
		{
			Environment.Exit(code);
		}
	}
}