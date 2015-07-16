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
			App.Initialize();
			Scene Scene = new Scene {Camera = new FreeCamera(new Transform {Position = new Vector3(0, 0, 0)}) {Speed = 30}};
			//grid			
			Scene.AddObject(new GameObject
			{
				Model = new Skydome { Texture = Config.GetDataPath("Galactic.jpg", "Sky") },
			});

			//bridge model from file
			Scene.AddObject(new GameObject
			{
				Model = Model.FromFile(@"Stone Bridge\bridge.obj", Effect.FromFile("TexEffect.hlsl"))
			});


			//20^3 cubes with one model
			var cube = Model.Create(new TextureMesh(Effect.FromFile("TexEffect.hlsl"), GeometryGenerator.CreateBox<TextureVertex>(10, 10, 10)));

			var r = new Random();
			for (var i = 0; i < 20; i++)
				for (var j = 0; j < 20; j++)
					for (var k = 0; k < 2; k++)
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
								var way = (Scene.Camera.Transform.Position - o.Transform.Position);
								way.X *= r.NextFloat(0.5f, 1.5f);
								way.Y *= r.NextFloat(0.5f, 1.5f);
								way.Z *= r.NextFloat(0.5f, 1.5f);
								way.Normalize();
								o.Transform.Position += way * 10 * r.NextFloat(0.5f, 2);
							}
						});
			Scene.Run();
		}
	}
}
