using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lain.Core;
using Lain.GAPI;
using SharpDX;

namespace Lain.Graphics.GUI
{
    public class Font
    {
        private static readonly Dictionary<string, Font> Fonts = new Dictionary<string, Font>();
        private int _textureFontSize, _lineHeight;
        private Texture _tex;
        private readonly Dictionary<int, CharDesc> _mChars = new Dictionary<int, CharDesc>();

        private Font()
        {
        }

        public byte FontSize { get; set; }

        public Point MeasureString(string sentence)
        {
            float x = 0;
            var numLetters = sentence.Length;
            var coef = (float) FontSize/_textureFontSize;
            var _char = new CharDesc();
            for (var i = 0; i < numLetters; i++)
            {
                _char = _mChars.Keys.Contains(sentence[i]) ? _mChars[sentence[i]] : _mChars[32];

                x += _char.XAdv*coef;
            }
            return new Point((int)(x /*+ _char.XOff * coef*/), (int)((_lineHeight) * coef));
        }

        public SpriteBatch.Sprite[] BuildSpriteArray(string sentence, Vector2 loc, Color4 color)
        {
            var numLetters = sentence.Length;
            var arr = new SpriteBatch.Sprite[numLetters];

            var coef = (float) FontSize/_textureFontSize;

            for (var i = 0; i < numLetters; i++)
            {
                var _char = _mChars.Keys.Contains(sentence[i]) ? _mChars[sentence[i]] : _mChars[32];

                var left = (loc.X + _char.XOff*coef)/Config.Width*2 - 1;
                var top = -((loc.Y + _char.YOff*coef)/Config.Height*2 - 1);
                var w = (_char.SrcW*coef)/Config.Width*2;
                var h = - ((_char.SrcH*coef)/Config.Height*2);

                arr[i] = new SpriteBatch.Sprite
                {
                    Tech = "SDF",
                    Texture = _tex,
                    Bitmap = new SpriteBatch.Bitmap
                    {
                        Color = color,
                        Destination = new RectangleF(left, top, w, h),
                        Source =
                            new RectangleF((float) _char.SrcX/_tex.Width, (float) _char.SrcY/_tex.Height,
                                (float) _char.SrcW/_tex.Width, (float) _char.SrcH/_tex.Height)
                    }
                };

                loc = new Vector2(loc.X + _char.XAdv*coef, loc.Y);
            }
            return arr;
        }

        public static Font FromFile(string fontFilename, byte fontSyze = 16)
        {
            if (Fonts.ContainsKey(fontFilename)) return Fonts[fontFilename];
            var font = new Font {FontSize = fontSyze};
            using (
                var fin = new StreamReader(FileSystem.Load(FileSystem.GetPath("font.fnt", "Fonts", fontFilename))))
            {
                while (!fin.EndOfStream)
                {
                    var line = fin.ReadLine().Split(new[] {' ', '=', '"'}, StringSplitOptions.RemoveEmptyEntries);
                    switch (line[0])
                    {
                        case "info":
                            for (var index = 1; index < line.Length; index++)
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
                        case "common":
                            for (var index = 1; index < line.Length; index++)
                            {
                                switch (line[index])
                                {
                                    case "lineHeight":
                                    {
                                        font._lineHeight = byte.Parse(line[index + 1]);
                                        break;
                                    }
                                }
                            }
                            break;
                        case "page":
                            for (var index = 1; index < line.Length; index++)
                            {
                                if (line[index] != "file") continue;
                                font._tex = FileSystem.GetPath(line[index + 1], "Fonts", fontFilename);
                                break;
                            }
                            break;
                        case "char":
                        {
                            ushort charId = 0;
                            var chard = new CharDesc();

                            for (var index = 1; index < line.Length; index++)
                            {
                                switch (line[index])
                                {
                                    case "id":
                                        ushort.TryParse(line[index + 1], out charId);
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
                            font._mChars.Add(charId, chard);
                        }
                            break;
                    }
                }
            }
            Fonts.Add(fontFilename, font);
            return Fonts[fontFilename];
        }

        private struct CharDesc
        {
            public short SrcH; //
            public short SrcW; //Размер
            public short SrcX; //Позиция
            public short SrcY; //
            public short XAdv; //
            public short XOff; //Опции
            public short YOff; //
        }
    }
}