#region Using

using SharpDX;
using System;
using System.IO;
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
        private readonly FreeCamera _freeCamera;
        private readonly DxModel _plos;
        private readonly DxModel _model = new DxModel();

        public MainWindow()
        {
            InitializeComponent();

            _freeCamera = new FreeCamera(new Vector3(0, 25, -150), 0, 0, true);
            DirectX11.Initialize(dxPanel.Handle, _freeCamera, "config.ini", true);
            
            _plos = GeometryGenerator.GenereateModel<TextureVertex>(GeometryGenerator.CreateGrid(1000,1000,3,3));

            Shown += (sender, args) => dxPanel.Run(Update, Draw);
        }

        public void Draw()
        {
            _plos.Render();
            _model.Render();

            toolStripStatusLabel1.Text = dxPanel.FPS + "\r" +_freeCamera.Position + "\r"+ Input.MouseState.Vector;
        }

        public new void Update()
        {
            Input.SetMouseCoord(WinAPI.GetCurPos());

            _freeCamera.RotateWithMouse();

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

            _freeCamera.Update();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            _freeCamera.RotateFactor = (sender as TrackBar).Value / 1000f;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            _freeCamera.Speed = (sender as TrackBar).Value / 10f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var te = new TextureEffect();
            te.InitializeFromFile("Texture.fx");
            EffectManager.Effects[1] = te;
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var path = Path.Combine(Application.StartupPath, "Data\\Models\\Paris2010_0.obj");
            _model.AsyncLoadModel(path);
        }

    }
}