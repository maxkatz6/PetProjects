using System;
using Ormeli;
using Ormeli.GAPI;
using Ormeli.Graphics;
using Ormeli.Graphics.Cameras;
using Ormeli.Graphics.Drawable;
using SharpDX;

namespace Game
{
	class Program
	{
		static void Main()
		{
			Scene Scene = new Scene
			{
				Camera = new FreeCamera(new Transform {Position = new Vector3(0, 50, 0)}) {Speed = 30},
				Skydome = new Skydome {Texture = Config.GetDataPath("Sky_68.jpg", "Sky")}
			};	

			//bridge model from file
			Scene.AddObject(new GameObject
			{
				Model = Model.FromFile(@"Stone Bridge\bridge.obj", Effect.FromFile("TexEffect.hlsl"))
			});

			Scene.AddObject(new GameObject
			{
				Model = Model.Create(new TextureMesh(Effect.FromFile("TexEffect.hlsl"), GeometryGenerator.CreateGrid<TextureVertex>(25000, 25000)))
			});

			//20^3 cubes with one model
			var cube = Model.Create(new TextureMesh(Effect.FromFile("TexEffect.hlsl"), GeometryGenerator.CreateBox<TextureVertex>(10, 10, 10)));

			var r = new Random();
			for (var i = 0; i < 15; i++)
				for (var j = 0; j < 15; j++)
					for (var k = 0; k < 15; k++)
						Scene.AddObject(new GameObject
						{
							Model = cube,
							Transform = new Transform
							{
								Position = new Vector3(r.NextFloat(-1, 1), r.NextFloat(-1, 1), r.NextFloat(-1, 1)) * 40000,
								Rotation = new Vector3(r.Next(0, 3), r.Next(0, 3), r.Next(0, 3)),
								Scale = new Vector3(r.Next(1, 10))
							},
							Script = o =>
							{
								if (Input.IsKeyDown(Input.Key.D1))
									o.Transform.Position = new Vector3(r.NextFloat(-1, 1), r.NextFloat(-1, 1), r.NextFloat(-1, 1))*40000;
                                var way = (Scene.Camera.Transform.Position+500 - o.Transform.Position);
								way.Normalize();
								o.Transform.Position += way * 10 * r.NextFloat(0.5f, 2);
							}
						});
			Scene.Run();
		}
	}
}
