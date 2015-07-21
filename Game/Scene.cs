using System;
using System.Collections.Generic;
using Lain;
using Lain.Core.Patterns;
using Lain.GAPI;
using Lain.Graphics;
using Lain.Graphics.Cameras;
using Lain.Graphics.Drawable;
using SharpDX;

namespace Game
{
	public class Scene : Disposable
	{
		private readonly List<GameObject> _objects = new List<GameObject>
		{
			new GameObject
			{
				Model = Model.FromFile(@"Stone Bridge\bridge.obj", Effect.FromFile("TexEffect.hlsl"))
			},
			new GameObject
			{
				Model =
					Model.Create(new TextureMesh(Effect.FromFile("TexEffect.hlsl"),
						GeometryGenerator.CreateGrid<TextureVertex>(25000, 25000)))
			}
		};
		public FreeCamera Camera = new FreeCamera(new Transform {Position = new Vector3(0, 50, 0)});
        public Skydome Skydome = new Skydome { Texture = Config.GetDataPath("Sky_68.jpg", "Sky") };

		
		public void DrawAll()
		{
			Skydome.Draw(Camera.ViewRotation);

            foreach (var m in _objects)
                m.Model?.Draw(m.Transform != null ? m.Transform.WorldMatrix * Camera.ViewProjection : Camera.ViewProjection);
		}

		public void Update()
		{
			if (Input.IsKeyDown(Input.Key.Escape)) App.Exit();
			Console.Title = App.Window.FPS + " " + Input.Mouse;

			if (Input.IsKeyDown(Input.Key.W))
				Camera.DirectionMove(Vector3.ForwardRH);
			if (Input.IsKeyDown(Input.Key.S))
				Camera.DirectionMove(Vector3.BackwardRH);
			if (Input.IsKeyDown(Input.Key.A))
				Camera.DirectionMove(Vector3.Left);
			if (Input.IsKeyDown(Input.Key.D))
				Camera.DirectionMove(Vector3.Right);

			Camera.Update();
			foreach (var m in _objects)
				m.Script?.Invoke(m);
		}

		static void Main()
		{
			using (var scene = new Scene())
				App.Window.Run(scene.DrawAll, scene.Update);
		}
	}
}