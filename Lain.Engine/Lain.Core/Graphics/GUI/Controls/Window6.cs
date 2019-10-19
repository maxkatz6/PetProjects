using System;
using System.Collections.Generic;
using Lain.Core;
using Squid;
using Squid.Controls;
using Squid.Structs;
using Squid.Util;

namespace Lain.Graphics.GUI.Controls
{
    /// <summary>
    /// sample window 6 - ListView
    /// </summary>
    public class Window6 : MyWindow
    {
        public class MyData
        {
            public string Name;
            public DateTime Date;
            public int Rating;
        }

        public Window6()
        {
            try
            {

            Size = new Point(440, 340);
            Position = new Point(960, 400);
            Resizable = true;
            Titlebar.Text = "Misc";

            var rnd = new Random();
            var models = new List<MyData>();
            for (int i = 0; i < 32; i++)
            {
                var data = new MyData
                {
                    Name =
                        "[color=ff005577]" + rnd.Next() + " [/color][color=ff235577]" + rnd.Next() +
                        "[/color] " + rnd.Next(),
                    Date = DateTime.Now.AddMilliseconds(rnd.Next()),
                    Rating = rnd.Next()
                };
                models.Add(data);
            }

            var view = new ListView
            {
                Dock = DockStyle.Fill,
                StretchLastColumn = true,
                FullRowSelect = true
            };
            view.Columns.Add(new ListView.Column { Text = "Name", Aspect = "Name", Width = 120, MinWidth = 48 });
            view.Columns.Add(new ListView.Column { Text = "Date", Aspect = "Date", Width = 120, MinWidth = 48 });
            view.Columns.Add(new ListView.Column { Text = "Rating", Aspect = "Rating", Width = 120, MinWidth = 48 });
            Controls.Add(view);

            view.CreateHeader = (sender, args) =>
            {
                var header = new Button
                {
                    Dock = DockStyle.Fill,
                    Text = args.Column.Text,
                    AllowDrop = true
                };

                header.MouseClick += (snd, e) =>
                {
                    switch (args.Column.Aspect)
                    {
                        case "Name":
                            view.Sort<MyData>((a, b) => a.Name.CompareTo(b.Name));
                            break;
                        case "Date":
                            view.Sort<MyData>((a, b) => a.Date.CompareTo(b.Date));
                            break;
                        case "Rating":
                            view.Sort<MyData>((a, b) => a.Rating.CompareTo(b.Rating));
                            break;
                    }
                };

                header.MouseDrag += (snd, e) =>
                {
                    var drag = new Label
                    {
                        Size = snd.Size,
                        Position = snd.Location,
                        Style = snd.Style,
                        Text = ((Button) snd).Text
                    };

                    snd.DoDragDrop(drag);
                };

                header.DragLeave += (snd, e) => snd.Tint = -1;
                header.DragEnter += (snd, e) =>
                {
                    if (e.Source is Button)
                    {
                        snd.Tint = ColorInt.ARGB(1f, 0, 1f, 0);
                    }
                    else
                    {
                        snd.Tint = ColorInt.ARGB(1f, 1f, 0, 0);
                        e.Cancel = true;
                    }
                };

                header.DragDrop += delegate(Control snd, DragDropEventArgs e) { snd.Tint = -1; };

                return header;
            };
                
            view.CreateCell = (sender, args) =>
            {
                var text = view.GetAspectValue(args.Model, args.Column);
                /*                var field = args.Model.GetType().GetField(args.Column.Aspect, BindingFlags.Public);
                string text = field != null ? field.GetValue(args.Model).ToString() : "null";*/
                var cell = new Button
                {
                    Size = new Point(26, 26),
                    Style = "label",
                    Dock = DockStyle.Top,
                    Text = text,
                    TextAlign = Alignment.MiddleRight,
                    Tooltip = text,
                    AllowDrop = true,
                    BBCodeEnabled = true
                };

                //if (args.Column.Aspect == "Name")
                //    cell.TextAlign = Alignment.MiddleRight;
                if (args.Column.Aspect == "Date")
                    cell.TextAlign = Alignment.MiddleLeft;

                cell.DragResponse += (snd, e) => snd.State = ControlState.Hot;

                return cell;
            };

                Log.Write("3");
                view.SetObjects(models);
                Log.Write("4");
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Log.SendError(ex);
            }
        }
    }
}
