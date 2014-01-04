using System.Windows.Forms;
using SharpDX;
using TDF.Core;
using TDF.Graphics.Cameras;
using TDF.Graphics.Data;
using TDF.Graphics.Effects;
using TDF.Graphics.Models;
using TDF.Graphics.Render;
using TDF.Inputs;

namespace SimpleEffect
{
    public partial class Form1 : Form
    {
        private readonly FreeCamera _freeCamera;
        private  DxModel _model;

        public Form1()
        {
            InitializeComponent();
            _freeCamera = new FreeCamera(new Vector3(0, 25, -150), 0, 0, true);
            DirectX11.Initialize(directXPanel1.Handle, _freeCamera, true);
            var te = new TextureEffect();
            te.InitializeFromFile("Grayscale.fx");
    //        EffectManager.Effects[1] = te;
            _model = GeometryGenerator.GenereateModel<TextureVertex>(GeometryGenerator.CreateGrid(1000, 1000, 20, 20), new Texture("dragonTexture.dds"));

            Shown += (sender, args) => directXPanel1.Run(Update, Draw);
        }

        public void Draw()
        {
            _model.Render();
            label1.Text = directXPanel1.FPS.ToString();
        }

        public new void Update()
        {
            Input.SetMouseCoord(WinAPI.GetCurPos());

            _freeCamera.RotateWithMouse(Input.MouseState);

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
    }
}
