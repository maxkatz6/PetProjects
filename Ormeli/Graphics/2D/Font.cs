using System;
using System.Collections.Generic;
using System.IO;
using Ormeli.Graphics.Cameras;
using Ormeli.Graphics.Effects;
using Ormeli.Math;

namespace Ormeli.Graphics
{
    public class Font
    {
        private BitmapCharacterSet _charSet ;

    //    private static FontEffect fontEffect;

        public Texture Texture
        {
            get { return Texture.Textures[_texNum]; }
        }

        private int _texNum;

        private string lastString = string.Empty;
        private Buffer vb, ib;
        public void PrintText(string text, int x, int y, Color color)
        {
            if (lastString != text)
            {
                var texVec = new Vector2(_charSet.Width, _charSet.Height);
                var bv = new List<BitmapVertex>();
                var bi = new int[text.Length*6];
                int posX = 0;
                int ind = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    var v = _charSet.Characters[text[i]];

                    int left = posX + (-(Config.Width >> 1));
                    int right = left + v.Width;
                    int top = (Config.Height >> 1);
                    int bottom = top - v.Height;

                    bv.Add(new BitmapVertex(new Vector2(left, top), new Vector2(v.X, v.Y)/texVec));
                    bv.Add(new BitmapVertex(new Vector2(right, bottom), new Vector2(v.X + v.Width, v.Y)/texVec));
                    bv.Add(new BitmapVertex(new Vector2(left, bottom), new Vector2(v.X + v.Width, v.Y)/texVec));
                    bv.Add(new BitmapVertex(new Vector2(right, top), new Vector2(v.X + v.Width, v.Y)/texVec));

                    bi[6*i + 0] = i*ind + 0;
                    bi[6*i + 1] = i*ind + 1;
                    bi[6*i + 2] = i*ind + 2;
                    bi[6*i + 3] = i*ind + 0;
                    bi[6*i + 4] = i*ind + 2;
                    bi[6*i + 5] = i*ind + 3;
                    ind += 4;

                    posX += v.Width;
                }
                vb = App.Creator.CreateBuffer(bv.ToArray(), BindFlag.VertexBuffer, BufferUsage.Default,
                    CpuAccessFlags.None);
                ib = App.Creator.CreateBuffer(bi, BindFlag.IndexBuffer, BufferUsage.Default, CpuAccessFlags.None);
                lastString = text;
            }
            EffectBase.Get<Effect2D>(1).SetTexture(_texNum);
            EffectBase.Get<Effect2D>(1).SetMatrix(Matrix.Multiply(Matrix.Translation(x, -y, 0), Camera.Current.Ortho));
            EffectBase.Get<Effect2D>(1).Render("Texture", text.Length * 6);
            App.Render.SetBuffers(vb, ib, BitmapVertex.SizeInBytes);
            App.Render.Draw(text.Length * 6);

        }

        public void Initialize(string conf)
        {
            _charSet.Characters = new Dictionary<int, BitmapCharacter>(256);
           // fontEffect = new FontEffect("FontEffect.cgfx");
            using (var stream = new StreamReader(Config.GetDataPath(conf, "Font")))
            {
                string line;
                var separators = new[] {' ', '='};
                while ((line = stream.ReadLine()) != null)
                {
                    string[] tokens = line.Split(separators);
                    switch (tokens[0])
                    {
                      /*  case "info":
                            for (int i = 1; i < tokens.Length; i++)
                            {
                                if (tokens[i] == "size")
                                {
                                    _charSet.RenderedSize = int.Parse(tokens[i + 1]);
                                }
                            }
                            break;*/
                        case "common":
                            for (int i = 1; i < tokens.Length; i++)
                            {
                                switch (tokens[i])
                                {
                                    /*case "lineHeight":
                                        _charSet.LineHeight = int.Parse(tokens[i + 1]);
                                        break;
                                    case "base":
                                        _charSet.Base = int.Parse(tokens[i + 1]);
                                        break;*/
                                    case "scaleW":
                                        _charSet.Width = int.Parse(tokens[i + 1]);
                                        break;
                                    case "scaleH":
                                        _charSet.Height = int.Parse(tokens[i + 1]);
                                        break;
                                }
                            }
                            break;
                             case "page":
                            for (int i = 1; i < tokens.Length; i++)
                            {
                                switch (tokens[i])
                                {
                                    case "file":
                                        _texNum = Texture.GetNumber(tokens[i + 1].Split(new []{'"'}, StringSplitOptions.RemoveEmptyEntries)[0]);
                                        break;
                                }
                            }
                            break;
                        case "char":
                        {
                            int index = 0;
                            var v = new BitmapCharacter();
                            for (int i = 1; i < tokens.Length; i++)
                            {
                                switch (tokens[i])
                                {
                                    case "id":
                                        index = int.Parse(tokens[i + 1]);
                                        break;
                                    case "x":
                                        v.X = int.Parse(tokens[i + 1]);
                                        break;
                                    case "y":
                                        v.Y = int.Parse(tokens[i + 1]);
                                        break;
                                    case "width":
                                        v.Width = int.Parse(tokens[i + 1]);
                                        break;
                                    case "height":
                                        v.Height = int.Parse(tokens[i + 1]);
                                        break;
                                 /* case "xoffset":
                                        _charSet.Characters[index].XOffset = int.Parse(tokens[i + 1]);
                                        break;
                                    case "yoffset":
                                        _charSet.Characters[index].YOffset = int.Parse(tokens[i + 1]);
                                        break;
                                    case "xadvance":
                                        _charSet.Characters[index].XAdvance = int.Parse(tokens[i + 1]);
                                        break;*/
                                }
                            }
                            _charSet.Characters.Add(index, v);
                        }
                            break;
                     /* case "kerning":
                        {
                            // Build kerning list
                            int index = 0;
                            var k = new Kerning();
                            for (int i = 1; i < tokens.Length; i++)
                            {
                                switch (tokens[i])
                                {
                                    case "first":
                                        index = int.Parse(tokens[i + 1]);
                                        break;
                                    case "second":
                                        k.Second = int.Parse(tokens[i + 1]);
                                        break;
                                    case "amount":
                                        k.Amount = int.Parse(tokens[i + 1]);
                                        break;
                                }
                            }
                            _charSet.Characters[index].KerningList.Add(k);
                        }
                            break;*/
                    }
                }
            }
        }
    }

    internal struct BitmapCharacterSet
    {
     /*   public int LineHeight;
        public int Base;
        public int RenderedSize;
        public int Width;
        public int Height;*/
        public Dictionary<int, BitmapCharacter> Characters;
        public int Width;
        public int Height;
    }

    public struct BitmapCharacter 
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
    /*    public int XOffset;
        public int YOffset;
        public int XAdvance;*/
      //  public List<Kerning> KerningList = new List<Kerning>();
    }

    public class Kerning
    {
        public int Second;
        public int Amount;
    }

}
