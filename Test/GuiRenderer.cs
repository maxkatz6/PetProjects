using System;
using Squid;
using System.Collections.Generic;
using Lain.GAPI;
using SharpDX;
using Lain.Graphics.GUI;
using Font = Lain.Graphics.GUI.Font;

namespace Test
{
    public class GuiRenderer : ISquidRenderer
    {
        SpriteBatch SpriteBatch = new SpriteBatch();
        List<Texture> textures = new List<Texture>();
        List<Font> fonts = new List<Font>();
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void DrawBox(int x, int y, int width, int height, int color)
        {
            //throw new NotImplementedException();
        }

        public void DrawText(string text, int x, int y, int font, int color)
        {
            SpriteBatch.DrawText(fonts[font], text, new Vector2(x, y), Color.FromAbgr(color));
        }

        public void DrawTexture(int texture, int x, int y, int width, int height, Squid.Rectangle source, int color)
        {
            SpriteBatch.Draw(textures[texture], new RectangleF(x, y, width, height), new RectangleF(source.Left, source.Right, source.Right - source.Left, source.Bottom - source.Top));
        }

        public void EndBatch(bool final)
        {
            SpriteBatch.End();
        }

        public int GetFont(string name)
        {
            if (name == "default") name = "Arial";
            var fnt = Font.FromFile(name);
            var index = fonts.IndexOf(fnt);
            if (index >= 0) return index;
            else
            {
                fonts.Add(fnt);
                return textures.Count - 1;
            }
        }

        public Squid.Point GetTextSize(string text, int font)
        {
            return new Squid.Point();
        }

        public int GetTexture(string name)
        {
            var tex = Texture.FromFile(name);
            var index = textures.IndexOf(tex);
            if (index >= 0) return index;
            else
            {
                textures.Add(tex);
                return textures.Count - 1;
            }
        }

        public Squid.Point GetTextureSize(int texture)
        {
            return new Squid.Point(textures[texture].Width, textures[texture].Height);
        }

        public void Scissor(int x, int y, int width, int height)
        {
            //throw new NotImplementedException();
        }

        public void StartBatch()
        {
            SpriteBatch.Begin();
        }
    }
}
