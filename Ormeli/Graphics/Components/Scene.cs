using System;
using System.Collections.Generic;
using Ormeli.Core.Patterns;
using Ormeli.Graphics.Builders;
using Ormeli.Graphics.Cameras;
using Ormeli.Graphics.Drawable;
using SharpDX;

namespace Ormeli.Graphics
{
	public class Scene : Disposable
	{
		private readonly List<GameObject> _objects = new List<GameObject>(10000);
		public Camera Camera { get; set; }

		public void Run()
		{
			Console.WriteLine(_objects.Count);
			App.Loop.Run(DrawAll, Update);
		}

		public void AddObject(GameObject obj)
		{
			_objects.Add(obj);
		}

		public void LoadObject(string file, Transform tr = null, Action<GameObject> act = null)
		{
			Model model = ModelBuilder.Create().LoadFromFile(file);
			_objects.Add(new GameObject {Model = model, Transform = tr ?? new Transform(), Script = act});
		}

		public void DrawAll()
		{
			foreach (var m in _objects)
				m.Model?.Draw(m.Transform.WorldMatrix*Camera.ViewProjection);
		}

		public void Update()
		{
			if (Input.IsKeyDown(Input.Key.Escape)) App.Exit();
			Console.Title = App.Loop.FPS + " " + Input.Mouse;

			((FreeCamera) Camera).MoveWASD();

			foreach (var m in _objects)
				m.Script?.Invoke(m);
			Camera.Update();
		}

		//TODO Временно.
		public void Load()
		{
			Camera = new FreeCamera(new Transform {Position = new Vector3(0, 500, 0)}) {Speed = 30};
			//grid
			AddObject(new GameObject
			{
				Model = ModelBuilder.Create().
					SetMeshes(new TextureMesh("TexEffect.hlsl", GeometryGenerator.CreateGrid<TextureVertex>(1000, 1000))),
				Transform = new Transform {Position = new Vector3(0, -2000, 0)}
			});

			//bridge model from file
			LoadObject(@"Stone Bridge\bridge.obj");
			//LoadObject(@"Paris\Paris2010_0.obj", new Transform {Scale = new Vector3(100)});

			//20^3 cubes with one model
			var cube = ModelBuilder.Create().
				SetMeshes(new TextureMesh("TexEffect.hlsl", GeometryGenerator.CreateBox<TextureVertex>(10, 10, 10)));

			var r = new Random();
			for (var i = 0; i < 20; i++)
				for (var j = 0; j < 20; j++)
					for (var k = 0; k < 2; k++)
						AddObject(new GameObject
						{
							Model = cube,
							Transform = new Transform
							{
								Position = new Vector3(r.NextFloat(-1, 1), r.NextFloat(-1, 1), r.NextFloat(-1, 1))*40000,
								Rotation = new Vector3(r.Next(0, 3), r.Next(0, 3), r.Next(0, 3)),
								Scale = new Vector3(r.Next(1, 10))
							},
							Script = o =>
							{
								var way = (Camera.Transform.Position - o.Transform.Position);
								way.X *= r.NextFloat(0.5f, 1.5f);
								way.Y *= r.NextFloat(0.5f, 1.5f);
								way.Z *= r.NextFloat(0.5f, 1.5f);
								way.Normalize();
								o.Transform.Position += way*20*r.NextFloat(0.5f, 2);
							}
						});
		}
	}
}