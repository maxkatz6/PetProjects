#region Using

using System.Linq;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using TDF.Core;
using TDF.Graphics.Cameras;
using TDF.Graphics.Data;
using TDF.Graphics.Effects;
using TDF.Graphics.Models;
using TDF.Graphics.Render;
using TDF.Inputs;

#endregion Using

namespace TDFExample_ModelConverter.Core
{
    public partial class MainWindow : Form
    {
        private readonly Fps _fps = new Fps();
        private readonly FreeCamera _freeCamera;
        private  DxModel _model = new DxModel();

        private Effect le;

        public MainWindow()
        {
            InitializeComponent();

            Resize +=
                (sender, args) => { if (splitContainer1.Panel1.Width != 200) splitContainer1.SplitterDistance = 200; };

            _freeCamera = new FreeCamera(new Vector3(0, 25, -150), 0, 0, true, true);
            DirectX11.Initialize(directXPanel1.Handle, _freeCamera, true);

             le = new TextureEffect();
            le.InitializeFromFile("Texture.fx");

            var m = GeometryGenerator.CreateGrid(1000, 1000, 2, 2).Convert<TextureVertex>().ToMesh();

            m.SetMaterial(new Material
            {
                DiffuseTexture = Texture.NullTexture,
            });

            m.Effect = le;
            _model.SetMeshes(m);

            Shown += (sender, args) => directXPanel1.Run(delegate {}, () =>
            {
                _fps.Frame();

                _freeCamera.Update();
                _freeCamera.RotateWithMouse(Input.MouseState);
                _model.Render();
                if (Input.IsKeyDown(Key.W))
                {
                    _freeCamera.DirectionMove(Vector3.ForwardLH);
                }
                else if (Input.IsKeyDown(Key.S))
                {
                    _freeCamera.DirectionMove(Vector3.BackwardLH);
                }

                if (Input.IsKeyDown(Key.A))
                {
                    _freeCamera.DirectionMove(Vector3.Left);
                }
                else if (Input.IsKeyDown(Key.D))
                {
                    _freeCamera.DirectionMove(Vector3.Right);
                }

                Point p = WinAPI.GetCurPos();
                Input.SetMouseCoord(p.X - directXPanel1.Location.X, p.Y - directXPanel1.Location.Y - 47);
                toolStripStatusLabel1.Text = _fps.Value.ToString(CultureInfo.InvariantCulture) + "      " +
                                             _freeCamera.Position + "                " + Input.MouseState.Vector;
            });
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            _freeCamera.RotateFactor = (sender as TrackBar).Value / 1000f;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            _freeCamera.Speed = (sender as TrackBar).Value / 10f;
        }
    }
}