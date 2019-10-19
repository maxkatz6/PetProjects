using System.Collections.Generic;
using Lain.GAPI;
using SharpDX;
using Squid.Interfaces;

namespace Lain.Graphics.GUI
{
    public class GuiRenderer : ISquidRenderer
    {
        private readonly Texture Blank = Texture.Create(new[,] {{Color4.White}});
        private readonly List<Font> fonts = new List<Font>();
        private readonly SpriteBatch SpriteBatch = new SpriteBatch();
        private readonly List<Texture> textures = new List<Texture>();

        public void Dispose()
        {
        }

        public void DrawBox(int x, int y, int width, int height, int color)
        {
            SpriteBatch.Draw(Blank, new RectangleF(x, y, width, height), null, Color.FromRgba(color));
        }

        public void DrawText(string text, int x, int y, int font, int color)
        {
            SpriteBatch.DrawText(fonts[font], text, new Vector2(x, y), Color.FromRgba(color));
        }

        public void DrawTexture(int texture, int x, int y, int width, int height, Squid.Structs.Rectangle sourceRec, int color)
        {
            var source = new RectangleF(sourceRec.Left, sourceRec.Top, sourceRec.Width, sourceRec.Height);
            SpriteBatch.Draw(textures[texture], new RectangleF(x, y, width, height), source, Color.FromRgba(color));
        }
        
        public void EndBatch(bool final)
        {
            SpriteBatch.End();
        }

        public int GetFont(string name)
        {
            if (name == null
                || name == "default")
                name = "Arial";
            var fnt = Font.FromFile(name);
            var index = fonts.IndexOf(fnt);
            if (index >= 0) return index;
            fonts.Add(fnt);
            return textures.Count - 1;
        }
        
        public Squid.Structs.Point GetTextSize(string text, int font)
        {
            if (font < 0 || font > fonts.Count) return Squid.Structs.Point.Zero;
            var p = fonts[font].MeasureString(text);
            return new Squid.Structs.Point(p.X, p.Y);
        }

        public int GetTexture(string name)
        {
            var index = textures.IndexOf(name);
            if (index >= 0) return index;
            textures.Add(name);
            return textures.Count - 1;
        }

        public Squid.Structs.Point GetTextureSize(int texture)
        {
            return new Squid.Structs.Point(textures[texture].Width, textures[texture].Height);
        }

        public void Scissor(int x, int y, int width, int height)
        {
            App.Render.SetScissor(x, y, x + width, y + height);
        }

        public void StartBatch()
        {
            SpriteBatch.Begin();
        }
    }
}