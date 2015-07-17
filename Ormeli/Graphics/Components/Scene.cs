using System;
using System.Collections.Generic;
using Ormeli.Graphics.Cameras;
using Ormeli.Graphics.Drawable;

namespace Ormeli.Graphics
{
	public class Scene 
	{
		private readonly List<GameObject> _objects = new List<GameObject>(1000);
		public Camera Camera { get; set; }
		public Skydome Skydome { get; set; }
		public void Run()
		{
			App.Window.Run(DrawAll, Update);
		}

		public void AddObject(GameObject obj)
		{
			_objects.Add(obj);
		}
		
		public void DrawAll()
		{
			Skydome?.Draw(Camera.ViewRotation);

			foreach (var m in _objects)
				m.Model?.Draw(m.Transform.WorldMatrix*Camera.ViewProjection);
		}

		public void Update()
		{
			if (Input.IsKeyDown(Input.Key.Escape)) App.Exit();
			Console.Title = App.Window.FPS + " " + Input.Mouse;

			((FreeCamera) Camera).MoveWASD();
			Camera.Update();
			foreach (var m in _objects)
				m.Script?.Invoke(m);
		}
	}
}