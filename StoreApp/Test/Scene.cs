using Lain;
using Lain.Core.Patterns;
using Lain.GAPI;
using Lain.Graphics;
using Lain.Graphics.Cameras;
using Lain.Graphics.Drawable;
using SharpDX;
using Lain.Graphics.GUI;
using Lain.Core;
using Lain.StoreApp;
using System;
using System.Collections.Generic;

namespace Test
{
    public class Scene : Disposable
    {
        private readonly List<GameObject> _objects = new List<GameObject>
        {
            new GameObject
            {
                Model =
                    Model.Create(new TextureMesh(Effect.FromFile("TexEffect.hlsl"),
                        GeometryGenerator.CreateGrid<TextureVertex>(25000, 25000)))
            }
        };
        FreeCamera Camera = new FreeCamera(new Transform { Position = new Vector3(0, 50, 0) });
        Skydome Skydome = new Skydome { Texture = FileSystem.GetPath("Sky_68.jpg", "Sky")};
        SpriteBatch sp = new SpriteBatch();
        Font f = Font.FromFile("Arial");
        IRenderSource Source;
        public Scene(IRenderSource rs)
        {
            Source = rs;
            var m = Model.Create(new TextureMesh(Effect.FromFile("TexEffect.hlsl"), GeometryGenerator.CreateBox<TextureVertex>(50, 50, 50)));
            var r = new Random();
            for (int i = 0; i < 500; i++)
                _objects.Add(new GameObject
                {
                    Model = m,
                    Transform = new Transform { Position = new Vector3(r.NextFloat(-1, 1) * 5000, r.NextFloat(0, 1) * 5000, r.NextFloat(-1, 1) * 5000) }
                });
            rs.Run(DrawAll, Update);
        }

        public void DrawAll()
		{
            Skydome.Draw(Camera.ViewRotation);

            foreach (var m in _objects)
                m.Model?.Draw(m.Transform != null ? m.Transform.WorldMatrix * Camera.ViewProjection : Camera.ViewProjection);

            sp.Begin();
            sp.Draw(FileSystem.GetPath("Sky_68.jpg", "Sky"), new RectangleF(0,0, 300, 300));
            sp.DrawText(f, "FPS: " + Source.FPS);
            sp.End();  
        }

		public void Update()
        {
			if (Input.IsKeyDown(Input.Key.W))
				Camera.DirectionMove(Vector3.ForwardRH);
			if (Input.IsKeyDown(Input.Key.S))
				Camera.DirectionMove(Vector3.BackwardRH);
			if (Input.IsKeyDown(Input.Key.A))
				Camera.DirectionMove(Vector3.Left);
			if (Input.IsKeyDown(Input.Key.D))
				Camera.DirectionMove(Vector3.Right);

			Camera.Update();
        }
	}
}