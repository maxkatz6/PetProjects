using System;
using System.Collections.Generic;
using Ormeli.Core.Patterns;
using Ormeli.Graphics.Cameras;

namespace Ormeli.Graphics
{
	public class Scene : Disposable
	{
		private readonly List<GameObject> _objects = new List<GameObject>(1000);
		public Camera Camera { get; set; }
		public void Run()
		{
			//App.Render.DeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineStrip;
			App.Window.Run(DrawAll, Update);
		}

		public void AddObject(GameObject obj)
		{
			_objects.Add(obj);
		}
		
		public void DrawAll()
		{
			_objects[0].Model.Draw(Camera.ViewRotation);
			for (int i = 1; i < _objects.Count; i++)
				_objects[i].Model?.Draw(_objects[i].Transform.WorldMatrix * Camera.ViewProjection);
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