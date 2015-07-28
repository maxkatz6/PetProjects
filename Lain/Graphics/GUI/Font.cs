using System;
using System.Collections.Generic;
using System.IO;
using SharpDX;
using System.Linq;
using Lain.GAPI;

namespace Lain.Graphics.GUI
{
    public class Font
    {
        struct CharDesc
        {
            public short SrcX; //Позиция
            public short SrcY; //
            public short SrcW; //Размер
            public short SrcH; //
            public short XOff; //Опции
            public short YOff; //
            public short XAdv; //
        }

        private static Dictionary<string, Font> fonts = new Dictionary<string, Font>();

        public byte FontSize { get; set; }
        private byte _textureFontSize;

        Texture tex;
        readonly Dictionary<int, CharDesc> m_Chars = new Dictionary<int, CharDesc>();

        private Font() { }

        public SpriteBatch.Sprite[] BuildSpriteArray(string sentence, Vector2 loc, Color4 color)
        {
            int numLetters = sentence.Length;
            var arr = new SpriteBatch.Sprite[numLetters];

            var coef = (float)FontSize / _textureFontSize;

            float deltaX = 1f / (tex.Width);
            float deltaY = 1f / (tex.Height);



            for (int i = 0; i < numLetters; i++)
            {
                var _char = m_Chars.Keys.Contains(sentence[i]) ? m_Chars[sentence[i]] : m_Chars[32];

                var left = (loc.X + _char.XOff * coef) / Config.Width * 2 - 1;
                var top = -((loc.Y + _char.YOff * coef) / Config.Height * 2 - 1);
                var w = (_char.SrcW * coef) / Config.Width * 2;
                var h = -((_char.SrcH * coef) / Config.Height * 2);

                arr[i] = new SpriteBatch.Sprite
                {
                    Texture = tex,
                    Bitmap = new SpriteBatch.Bitmap
                    {
                        Color = color,
                        Destination = new RectangleF(left, top, w, h),
                        Source = new RectangleF((float)_char.SrcX / tex.Width, (float)_char.SrcY / tex.Height, (float)_char.SrcW / tex.Width, (float)_char.SrcH / tex.Height)
                    }
                };

                loc = new Vector2(loc.X + _char.XAdv * coef, loc.Y);
            }
            return arr;

        }

        public static Font FromFile(string fontFilename, byte fontSyze = 32)
        {
            if (!fonts.ContainsKey(fontFilename))
            {
                var font = new Font { FontSize = fontSyze };
                using (var fin = new StreamReader(Config.GetDataPath("inf.fnt", "Fonts", fontFilename)))
                {
                    while (!fin.EndOfStream)
                    {
                        var line = fin.ReadLine().Split(new[] { ' ', '=', '"' }, StringSplitOptions.RemoveEmptyEntries);
                        switch (line[0])
                        {
                            /*case "common":
                                for (int index = 1; index < line.Length; index++)
                                {
                                    switch (line[index])
                                    {
                                        case "scaleW":
                                            {
                                                font._mWidthTex = ushort.Parse(line[index + 1]);
                                                break;
                                            }
                                        case "scaleH":
                                            {
                                                font._mHeightTex = ushort.Parse(line[index + 1]);
                                                break;
                                            }
                                    }
                                }
                                break;*/
                            case "info":
                                for (int index = 1; index < line.Length; index++)
                                {
                                    switch (line[index])
                                    {
                                        case "size":
                                            {
                                                font._textureFontSize = byte.Parse(line[index + 1]);
                                                break;
                                            }
                                    }
                                }
                                break;
                            case "page":
                                for (var index = 1; index < line.Length; index++)
                                {
                                    if (line[index] != "file") continue;
                                    font.tex = Config.GetDataPath(line[index + 1], "Fonts", fontFilename);
                                    break;
                                }
                                break;
                            case "char":
                                {
                                    ushort CharID = 0;
                                    var chard = new CharDesc();

                                    for (var index = 1; index < line.Length; index++)
                                    {
                                        switch (line[index])
                                        {
                                            case "id":
                                                ushort.TryParse(line[index + 1], out CharID);
                                                break;
                                            case "x":
                                                chard.SrcX = short.Parse(line[index + 1]);
                                                break;
                                            case "y":
                                                chard.SrcY = short.Parse(line[index + 1]);
                                                break;
                                            case "width":
                                                chard.SrcW = short.Parse(line[index + 1]);
                                                break;
                                            case "height":
                                                chard.SrcH = short.Parse(line[index + 1]);
                                                break;
                                            case "xoffset":
                                                chard.XOff = short.Parse(line[index + 1]);
                                                break;
                                            case "yoffset":
                                                chard.YOff = short.Parse(line[index + 1]);
                                                break;
                                            case "xadvance":
                                                chard.XAdv = short.Parse(line[index + 1]);
                                                break;
                                        }
                                    }
                                    font.m_Chars.Add(CharID, chard);
                                }
                                break;
                        }
                    }
                }
                fonts.Add(fontFilename, font);
            }
            return fonts[fontFilename];
        }
    }
}