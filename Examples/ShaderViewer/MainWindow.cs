#region Using

using System.Text.RegularExpressions;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TDF.Core;
using TDF.Graphics.Cameras;
using TDF.Graphics.Data;
using TDF.Graphics.Effects;
using TDF.Graphics.Models;
using TDF.Graphics.Render;
using TDF.Inputs;

#endregion Using

namespace ShaderViewer
{
    public partial class MainWindow : Form
    {
        private readonly FreeCamera _freeCamera;
        private readonly DxModel _plos;

        #region ketwords

        private List<string> _keywords = new List<string>
        {
            "AppendStructuredBuffer",
            "asm",
            "asm_fragment",
            "BlendState",
            "bool",
            "break",
            "Buffer",
            "ByteAddressBuffer",
            "case",
            "cbuffer",
            "centroid",
            "class",
            "column_major",
            "compile",
            "compile_fragment",
            "CompileShader",
            "const",
            "continue",
            "ComputeShader",
            "ConsumeStructuredBuffer",
            "default",
            "DepthStencilState",
            "DepthStencilView",
            "discard",
            "do",
            "double",
            "DomainShader",
            "dword",
            "else",
            "export",
            "extern",
            "false",
            "float",
            "for",
            "fxgroup",
            "GeometryShader",
            "groupshared",
            "half",
            "Hullshader",
            "if",
            "in",
            "inline",
            "inout",
            "InputPatch",
            "int",
            "interface",
            "line",
            "lineadj",
            "linear",
            "LineStream",
            "matrix",
            "min16float",
            "min10float",
            "min16int",
            "min12int",
            "min16uint",
            "namespace",
            "nointerpolation",
            "noperspective",
            "NULL",
            "out",
            "OutputPatch",
            "packoffset",
            "pass",
            "pixelfragment",
            "PixelShader",
            "point",
            "PointStream",
            "precise",
            "RasterizerState",
            "RenderTargetView",
            "return",
            "register",
            "row_major",
            "RWBuffer",
            "RWByteAddressBuffer",
            "RWStructuredBuffer",
            "RWTexture1D",
            "RWTexture1DArray",
            "RWTexture2D",
            "RWTexture2DArray",
            "RWTexture3D",
            "sample",
            "sampler",
            "SamplerState",
            "SamplerComparisonState",
            "shared",
            "snorm",
            "stateblock",
            "stateblock_state",
            "static",
            "string",
            "struct",
            "switch",
            "StructuredBuffer",
            "tbuffer",
            "technique",
            "technique10",
            "technique11",
            "texture",
            "Texture1D",
            "Texture1DArray",
            "Texture2D",
            "Texture2DArray",
            "Texture2DMS",
            "Texture2DMSArray",
            "Texture3D",
            "TextureCube",
            "TextureCubeArray",
            "true",
            "typedef",
            "triangle",
            "triangleadj",
            "TriangleStream",
            "uint",
            "uniform",
            "unorm",
            "unsigned",
            "vector",
            "vertexfragment",
            "VertexShader",
            "void",
            "volatile",
            "while"
        };

        private List<string> _registers = new List<string>
        {
            "\"",
            "'",
            "@",
        };

        #endregion ketwords

        public MainWindow()
        {
            InitializeComponent();

            _freeCamera = new FreeCamera(new Vector3(0, 25, -150), 0, 0, true);
            DirectX11.Initialize(directXPanel1.Handle, _freeCamera, true);

            _plos = GeometryGenerator.GenereateModel<TextureVertex>(GeometryGenerator.CreateGrid(1000, 1000, 3, 3));

            Shown += (sender, args) => directXPanel1.Run(Update, Draw);
            textBox1.Text = EffectManager.TextureShader;
            richTextBox1.Text = EffectManager.ColorShader;
            colorText(textBox1);
            colorText(richTextBox1 );
        }

        public void Draw()
        {
            _plos.Render();

            toolStripStatusLabel1.Text = directXPanel1.FPS + "\r" + _freeCamera.Position + "\r" +
                                         Input.MouseState.Vector;
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
            var te = new TextureEffect();
                te.InitializeFromMemory(textBox1.Text);
            EffectManager.Effects[1] = te;
            }
            catch
            {
                MessageBox.Show("Error if HLSL code");
            }
            colorText(textBox1);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            _freeCamera.RotateFactor = (sender as TrackBar).Value / 1000f;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            _freeCamera.Speed = (sender as TrackBar).Value / 10f;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var ce = new ColorEffect();
                ce.InitializeFromMemory(richTextBox1.Text);
                EffectManager.Effects[0] =ce;
            }
            catch
            {
                MessageBox.Show("Error if HLSL code");
            }
            colorText(richTextBox1);
        }

        public void colorText(RichTextBox rt)
        {
            foreach (var keyword in _keywords)
            {
                var allIp = Regex.Matches(rt.Text, keyword + " ");
                foreach (Match ip in allIp)
                {
                    rt.SelectionStart = ip.Index;
                    rt.SelectionLength = ip.Length;
                    rt.SelectionColor = System.Drawing.Color.Blue;
                }
            }
            foreach (var register in _registers)
            {
                var allIp = Regex.Matches(rt.Text, register);
                foreach (Match ip in allIp)
                {
                    rt.SelectionStart = ip.Index;
                    rt.SelectionLength = ip.Length;
                    rt.SelectionColor = System.Drawing.Color.OrangeRed;
                }
            }
        }
    }
}