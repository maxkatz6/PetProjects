using System;
using Lain.GAPI;

namespace Lain
{
	public static class App
	{
		/* Just DO IT! Keep It Simple, Stupid!
		 *          TODO Возможно
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