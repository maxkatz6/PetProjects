using System;
using Ormeli.GAPI;

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
		public static Window Window = Window.Create();
		internal static Render Render = Render.Create();

		public static void Exit()
		{
			Environment.Exit(0);
		}
	}
}