using System.Collections.Generic;
using System.IO;
using Lain.Core;
using Newtonsoft.Json;
using Squid.Controls;
using Squid.Skinning;
using Squid.Structs;
using Squid.Util;

namespace Lain.Graphics.GUI
{
    public static class SkinAtlas
    {
        public static void ApplySkin(this Desktop desk, string atlas)
        {
            var tex = FileSystem.GetPath(atlas + ".png", "Squid");
            var texRec = ParseAtlas(FileSystem.GetPath(atlas + ".atlas", "Squid"));

            var itemStyle = new ControlStyle
            {
                TextPadding = new Margin(8, 0, 8, 0),
                TextAlign = Alignment.MiddleLeft,
                Tiling = TextureMode.Grid,
                Grid = new Margin(3),
                Texture = tex,
                Default = { TextureRect = texRec["button_default.png"] },
                Pressed = { TextureRect = texRec["button_down.png"] },
                SelectedPressed = { TextureRect = texRec["button_down.png"] },
                Focused = { TextureRect = texRec["button_down.png"] },
                SelectedFocused = { TextureRect = texRec["button_down.png"] },
                Selected = { TextureRect = texRec["button_down.png"] },
                SelectedHot = { TextureRect = texRec["button_down.png"] }
            };

            var buttonStyle = new ControlStyle()
            {
                TextPadding = new Margin(0),
                TextAlign = Alignment.MiddleCenter,
                Tiling = TextureMode.Grid,
                Grid = new Margin(3),
                Texture = tex,
                Default = { TextureRect = texRec["button_default.png"] },
                Pressed = { TextureRect = texRec["button_down.png"] },
                SelectedPressed = { TextureRect = texRec["button_down.png"] },
                Focused = { TextureRect = texRec["button_down.png"] },
                SelectedFocused = { TextureRect = texRec["button_down.png"] },
                Selected = { TextureRect = texRec["button_down.png"] },
                SelectedHot = { TextureRect = texRec["button_down.png"] }
            };

            var tooltipStyle = new ControlStyle
            {
                TextPadding = new Margin(8),
                TextAlign = Alignment.TopLeft,
                Tiling = TextureMode.Grid,
                Grid = new Margin(3),
                Texture = tex,
                Default = { TextureRect = texRec["button_default.png"] },
                Pressed = { TextureRect = texRec["button_down.png"] },
                SelectedPressed = { TextureRect = texRec["button_down.png"] },
                Focused = { TextureRect = texRec["button_down.png"] },
                SelectedFocused = { TextureRect = texRec["button_down.png"] },
                Selected = { TextureRect = texRec["button_down.png"] },
                SelectedHot = { TextureRect = texRec["button_down.png"] }
            };

            var inputStyle = new ControlStyle
            {
                Grid = new Margin(3),
                Texture = tex,
                Hot = {TextureRect = texRec["input_focused.png"]},
                Focused = {TextureRect = texRec["input_focused.png"], Tint = ColorInt.ARGB(1f,1,0,0)},
                TextPadding = new Margin(8),
                Tiling = TextureMode.Grid
            };

            var windowStyle = new ControlStyle
            {
                Tiling = TextureMode.Grid,
                Grid = new Margin(9),
                Texture = tex,
                TextureRect = texRec["window.png"]
            };

            var frameStyle = new ControlStyle
            {
                Tiling = TextureMode.Grid,
                Grid = new Margin(4),
                Texture = tex,
                TextureRect = texRec["frame.png"],
                TextPadding = new Margin(8)
            };

            var vscrollTrackStyle = new ControlStyle
            {
                Tiling = TextureMode.Grid,
                Grid = new Margin(3),
                Texture = tex,
                TextureRect = texRec["vscroll_track.png"]
            };

            var vscrollButtonStyle = new ControlStyle
            {
                Tiling = TextureMode.Grid,
                Grid = new Margin(3),
                Texture = tex,
                TextureRect = texRec["vscroll_button.png"],
                Hot = { TextureRect = texRec["vscroll_button_hot.png"] },
                Pressed = { TextureRect = texRec["vscroll_button_down.png"] }
            };

            var vscrollUp = new ControlStyle
            {
                Texture = tex,
                Default = {TextureRect = texRec["vscrollUp_default.png"]},
                Hot = { TextureRect = texRec["vscrollUp_hot.png"] },
                Pressed = { TextureRect = texRec["vscrollUp_down.png"] },
                Focused = { TextureRect = texRec["vscrollUp_hot.png"] }
            };

            var hscrollTrackStyle = new ControlStyle
            {
                Tiling = TextureMode.Grid,
                Grid = new Margin(3),
                Texture = tex,
                TextureRect = texRec["hscroll_track.png"]
            };

            var hscrollButtonStyle = new ControlStyle
            {
                Tiling = TextureMode.Grid,
                Grid = new Margin(3),
                Texture = tex,
                TextureRect = texRec["hscroll_button.png"],
                Hot = {TextureRect = texRec["hscroll_button_hot.png"]},
                Pressed = {TextureRect = texRec["hscroll_button_down.png"]}
            };

            var hscrollUp = new ControlStyle
            {
                Texture = tex,
                Default = {TextureRect = texRec["hscrollUp_default.png"]},
                Hot = {TextureRect = texRec["hscrollUp_hot.png"]},
                Pressed = { TextureRect = texRec["hscrollUp_down.png"]},
                Focused = { TextureRect = texRec["hscrollUp_hot.png"]}
            };

            var checkButtonStyle = new ControlStyle
            {
                Texture = tex,
                Default = { TextureRect = texRec["checkbox_default.png"]},
                Hot = { TextureRect = texRec["checkbox_hot.png"]},
                Pressed = { TextureRect = texRec["checkbox_down.png"]},
                Checked = { TextureRect = texRec["checkbox_checked.png"]},
                CheckedFocused = { TextureRect = texRec["checkbox_checked_hot.png"]},
                CheckedHot = { TextureRect = texRec["checkbox_checked_hot.png"]},
                CheckedPressed = { TextureRect = texRec["checkbox_down.png"]}
            };

            var comboLabelStyle = new ControlStyle
            {
                TextPadding = new Margin(10, 0, 0, 0),
                Texture = tex,
                Default = { TextureRect = texRec["combo_default.png"]},
                Hot = { TextureRect = texRec["combo_hot.png"]},
                Pressed = { TextureRect = texRec["combo_down.png"]},
                Focused = { TextureRect = texRec["combo_hot.png"]},
                Tiling = TextureMode.Grid,
                Grid = new Margin(3, 0, 0, 0)
            };

            var comboButtonStyle = new ControlStyle
            {
                Texture = tex,
                Default = { TextureRect = texRec["combo_button_default.png"]},
                Hot = { TextureRect = texRec["combo_button_hot.png"]},
                Pressed = { TextureRect = texRec["combo_button_down.png"]},
                Focused = { TextureRect = texRec["combo_button_hot.png"]}
            };

            var multilineStyle = new ControlStyle {TextAlign = Alignment.TopLeft, TextPadding = new Margin(0)};

            var labelStyle = new ControlStyle
            {
                TextPadding = new Margin(8, 0, 8, 0),
                TextAlign = Alignment.MiddleLeft,
                TextColor = ColorInt.ARGB(1f, .8f, .8f, .8f),
                BackColor = 0,
                Hot = { BackColor = ColorInt.ARGB(.125f, 1f, 1f, 1f) }
            };

            var s = new Skin
            {
                {"item", itemStyle},
                {"textbox", inputStyle},
                {"button", buttonStyle},
                {"window", windowStyle},
                {"frame", frameStyle},
                {"checkBox", checkButtonStyle},
                {"comboLabel", comboLabelStyle},
                {"comboButton", comboButtonStyle},
                {"vscrollTrack", vscrollTrackStyle},
                {"vscrollButton", vscrollButtonStyle},
                {"vscrollUp", vscrollUp},
                {"hscrollTrack", hscrollTrackStyle},
                {"hscrollButton", hscrollButtonStyle},
                {"hscrollUp", hscrollUp},
                {"multiline", multilineStyle},
                {"tooltip", tooltipStyle},
                {"label", labelStyle}
            };
            desk.Skin = s;

           var  json = JsonConvert.SerializeObject(s);
            Log.Write(json);
            CursorCollection cc = new CursorCollection
            {
                {
                    CursorNames.Default,
                    new Cursor
                    {
                        Texture = tex,
                        Size = texRec["Arrow.png"].Size,
                        TextureRect = texRec["Arrow.png"],
                        HotSpot = Point.Zero
                    }
                },
                {
                    CursorNames.Link,
                    new Cursor
                    {
                        Texture = tex,
                        Size = texRec["Link.png"].Size,
                        TextureRect = texRec["Link.png"],
                        HotSpot = Point.Zero
                    }
                },
                {
                    CursorNames.Move,
                    new Cursor
                    {
                        Texture = tex,
                        Size = texRec["Move.png"].Size,
                        TextureRect = texRec["Move.png"],
                        HotSpot = texRec["Move.png"].Size/2
                    }
                },
                {
                    CursorNames.Select,
                    new Cursor
                    {
                        Texture = tex,
                        Size = texRec["Select.png"].Size,
                        TextureRect = texRec["Select.png"],
                        HotSpot = texRec["Select.png"].Size/2
                    }
                },
                {
                    CursorNames.SizeNS,
                    new Cursor
                    {
                        Texture = tex,
                        Size = texRec["SizeNS.png"].Size,
                        TextureRect = texRec["SizeNS.png"],
                        HotSpot = texRec["SizeNS.png"].Size/2
                    }
                },
                {
                    CursorNames.SizeWE,
                    new Cursor
                    {
                        Texture = tex,
                        Size = texRec["SizeWE.png"].Size,
                        TextureRect = texRec["SizeWE.png"],
                        HotSpot = texRec["SizeWE.png"].Size/2
                    }
                },
                {
                    CursorNames.HSplit,
                    new Cursor
                    {
                        Texture = tex,
                        Size = texRec["SizeNS.png"].Size,
                        TextureRect = texRec["SizeNS.png"],
                        HotSpot = texRec["SizeNS.png"].Size/2
                    }
                },
                {
                    CursorNames.VSplit,
                    new Cursor
                    {
                        Texture = tex,
                        Size = texRec["SizeWE.png"].Size,
                        TextureRect = texRec["SizeWE.png"],
                        HotSpot = texRec["SizeWE.png"].Size/2
                    }
                },
                {
                    CursorNames.SizeNESW,
                    new Cursor
                    {
                        Texture = tex,
                        Size = texRec["SizeNESW.png"].Size,
                        TextureRect = texRec["SizeNESW.png"],
                        HotSpot = texRec["SizeNESW.png"].Size/2
                    }
                },
                {
                    CursorNames.SizeNWSE,
                    new Cursor
                    {
                        Texture = tex,
                        Size = texRec["SizeNWSE.png"].Size,
                        TextureRect = texRec["SizeNWSE.png"],
                        HotSpot = texRec["SizeNWSE.png"].Size/2
                    }
                }
            };
            json = JsonConvert.SerializeObject(cc);
            Log.Write(json);
            desk.CursorSet = cc;
        }
        private static Dictionary<string, Rectangle> ParseAtlas(string s)
        {
            //name     X pos  Y pos   Width  Height   Xoffset  Yoffset  Orig W  Orig H   Rot
            Dictionary<string, Rectangle> texinfo = new Dictionary<string, Rectangle>();
            using (var sr = new StreamReader(FileSystem.Load(s)))
            {
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    var str = sr.ReadLine().Split(' ', '\t');
                    texinfo.Add(str[0], new Rectangle(int.Parse(str[1]), int.Parse(str[2]), int.Parse(str[3]), int.Parse(str[4])));
                }
            }
            return texinfo;
        }
    }
}
