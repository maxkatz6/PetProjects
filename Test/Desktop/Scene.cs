using Lain;
using Lain.Core.Patterns;
using Lain.GAPI;
using Lain.Graphics;
using Lain.Graphics.Cameras;
using Lain.Graphics.Drawable;
using Lain.Core;
using System;
using System.Collections.Generic;
using Lain.Desktop;
using Lain.Graphics.GUI.Controls;
using SampleGui;
using SharpDX;
using Squid;
using Squid.Controls;
using Squid.Structs;
using Point = Squid.Structs.Point;

namespace Test
{
    public class Scene : Disposable
    {
        private readonly List<GameObject> _objects = new List<GameObject>
        {
            new GameObject
            {
                Model = Model.Create(new TextureMesh(Effect.FromFile("TexEffect.hlsl"),
                    GeometryGenerator.CreateGrid<TextureVertex>(25000, 25000)))
            }
        };

        readonly FreeCamera _camera = new FreeCamera(new Transform { Position = new Vector3(0, 50, 0) });
        readonly Skydome _skydome = new Skydome { Texture = FileSystem.GetPath("Sky_68.jpg", "Sky") };
        readonly Desktop _desktop = new Desktop { ShowCursor = true, Size = new Squid.Structs.Point(Config.Width, Config.Height) };
        private IRenderSource renderSource;
        public Scene(IRenderSource rs)
        {
            renderSource = rs;
            var m = Model.Create(new TextureMesh(Effect.FromFile("TexEffect.hlsl"), GeometryGenerator.CreateBox<TextureVertex>(50, 50, 50)));
            var r = new Random();
            for (int i = 0; i < 500; i++)
                _objects.Add(new GameObject
                {
                    Model = m,
                    Transform = new Transform { Position = new Vector3(r.NextFloat(-1, 1) * 5000, r.NextFloat(0, 1) * 5000, r.NextFloat(-1, 1) * 5000) },
                    Script = o => o.Transform.Position += Vector3.ForwardLH / 10
                });

            // _desktop.ApplySkin("Def");
            var w = new MyWindow { Size = new Point(400, 300) };
            var s = new Slider
            {
                Size = new Point(300, 20),
                Position = new Point(50, 100),
                Button = { Size = new Point(20, 30), Style = "vscrollButton" },
                AutoScale = true,
                Minimum = 0f,
                Maximum = 100f,
                Style = "vscrollTrack",
                MinHandleSize = 32,
            };
            s.ValueChanged += sender => w.Titlebar.Text += "1";
            w.Controls.Add(s);

            w.Show(_desktop);
            new Window2().Show(_desktop);
            rs.Run(DrawAll, Update);
        }

        public void DrawAll()
        {
            _skydome.Draw(_camera.ViewRotation);

            foreach (var m in _objects)
                m.Model?.Draw(m.Transform != null ? m.Transform.WorldMatrix * _camera.ViewProjection : _camera.ViewProjection);

            _desktop.Draw();
        }

        public void Update()
        {
            Gui.TimeElapsed = 0.5f;
            foreach (var o in _objects)
                o.Script?.Invoke(o);
            if (_desktop.State != ControlState.Default)
            {
                if (Input.IsKeyDown(Input.Key.W))
                    _camera.DirectionMove(Vector3.ForwardRH);
                if (Input.IsKeyDown(Input.Key.S))
                    _camera.DirectionMove(Vector3.BackwardRH);
                if (Input.IsKeyDown(Input.Key.A))
                    _camera.DirectionMove(Vector3.Left);
                if (Input.IsKeyDown(Input.Key.D))
                    _camera.DirectionMove(Vector3.Right);

                _camera.Update();
            }
            Gui.SetButtons(Input.Mouse.LeftButton);
            _desktop.Update();
        }

        public static void Main(string[] args)
        {
            new Scene(new RenderSource());
        }
    }
}