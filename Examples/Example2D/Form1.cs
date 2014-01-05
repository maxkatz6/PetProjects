using System.Windows.Forms;
using Engine.Graphics.GUI;
using SharpDX;
using TDF.Graphics.Render;

namespace Example2D
{
    public partial class Form1 : Form
    {
        Box box = new Box();
        Bitmap bit = new Bitmap();

        public Form1()
        {
            InitializeComponent();
            DirectX11.Initialize(directXPanel1.Handle, true);

            box.Initialize(new Size2(200,200), new Color4(1,1,0,1),new Point(500,30));
            bit.Initialize(new Point(20,20));

            Shown += (sender, args) => directXPanel1.Run(Update, Draw);

            DirectX11.TurnZBufferOff();
        }

        public void Draw()
        {
            box.Render();
            bit.Render();
            label1.Text = directXPanel1.FPS.ToString();
        }

        public new void Update()
        {
        }
    }
}
