using System;
using System.Windows.Forms;
using Engine.Graphics.GUI;
using SharpDX;
using TDF.Core;
using TDF.Graphics.Render;

namespace Example2D
{
    public partial class MainForm : Form
    {
        Box box = new Box();
        Bitmap bit = new Bitmap();

        private double i;

        public MainForm()
        {
            InitializeComponent();
            DirectX11.Initialize(directXPanel1.Handle, "config.ini", true);

            Height = Config.Height;
            Width = Config.Width;

            box.Initialize(new Size2(200,200), new Color4(1,1,0,1),new Point(500,30));
            bit.Initialize(new Point(20,20));

            Shown += (sender, args) => directXPanel1.Run(Update, Draw);

            DirectX11.TurnZBufferOff();
        }

        public void Draw()
        {
            box.Render((int)(50 * Math.Cos(i+180) + 100), (int)(50 * Math.Sin(i+180) + 100));
            bit.Render((int)(100 * Math.Cos(i) + Config.Width / 2 - 100), (int)(100 * Math.Sin(i) + Config.Height / 2 - 100));
            label1.Text = directXPanel1.FPS.ToString();
        }

        public new void Update()
        {
            i += 0.1;
        }
    }
}
