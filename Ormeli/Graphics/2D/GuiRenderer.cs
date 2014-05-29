using System;
using System.Collections.Generic;
using Ormeli.Core.Patterns;
using Ormeli.Math;
using Squid;
using Point = Squid.Point;
using Rectangle = Squid.Rectangle;

namespace Ormeli.Graphics
{
    public class GuiRenderer : Disposable, ISquidRenderer
    {
        private Dictionary<int, Font> FontById = new Dictionary<int, Font>();
        private Dictionary<string, int> FontIdByName = new Dictionary<string, int>();

        private int tempId;

        private const string DefaultFont = "font.fnt";

     //   private SpriteBatch Batch;

        public int GetTexture(string name)
        {
            return Texture.GetNumber(name);
        }

        public int GetFont(string name)
        {
            if (name == "default") name = DefaultFont;

            if (FontIdByName.TryGetValue(name, out tempId))
                return tempId;

            var font = new Font();
            font.Initialize(name);

            int index = FontIdByName.Count;

            FontIdByName.Add(name, index);
            FontById.Add(index, font);

            return index;
        }

        public Point GetTextSize(string text, int font)
        {
            return new Point();
        }

        public Point GetTextureSize(int texture)
        {
            var tex = Texture.Textures[texture];
            return new Point(tex.Width, tex.Height);
        }

        public void Scissor(int x, int y, int width, int height)
        {//wtf

        }

        private Color ColorFromtInt32(int color)
        {
            Byte[] bytes = BitConverter.GetBytes(color);
            return new Color(bytes[2], bytes[1], bytes[0], bytes[3]);
        }

        public void DrawBox(int x, int y, int width, int height, int color)
        {
        }

        public void DrawText(string text, int x, int y, int font, int color)
        {
            FontById[font].PrintText(text, x, y, ColorFromtInt32(color));
        }

        public void DrawTexture(int texture, int x, int y, int w, int h, Rectangle source, int color)
        {
            var destination = new Math.Rectangle(x, y, w, h);
            var s = new Math.Rectangle {X = source.Left, Y = source.Top, Width = source.Width, Height = source.Height};

    //        Batch.Draw(texture, destination, s, ColorFromtInt32(color));
        }

        public bool TranslateKey(int scancode, ref char character)
        {
            return true;
        }

        public void StartBatch()
        {
            App.Render.ZBuffer(false);
        }

        public void EndBatch(bool final)
        {
            App.Render.ZBuffer(true);
        }
    }
}
