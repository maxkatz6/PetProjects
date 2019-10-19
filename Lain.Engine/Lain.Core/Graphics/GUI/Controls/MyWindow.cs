using Squid.Controls;
using Squid.Structs;
using Squid.Util;

namespace Lain.Graphics.GUI.Controls
{
    public class TitleBar : Label
    {
        public Button CloseButton { get; }

        public TitleBar()
        {
            CloseButton = new Button
            {
                Size = new Point(30, 30),
                Style = "button",
                Tooltip = "Close Window",
                Dock = DockStyle.Right,
                Margin = new Margin(0, 8, 8, 8)
            };
            Elements.Add(CloseButton);
        }
    }

    public class MyWindow : Window
    {
        public TitleBar Titlebar { get; }

        public MyWindow()
        {
            AllowDragOut = true;
            Padding = new Margin(4);

            Titlebar = new TitleBar
            {
                Dock = DockStyle.Top,
                Size = new Point(122, 35),
                Cursor = CursorNames.Move,
                Style = "frame",
                Margin = new Margin(0, 0, 0, -1),
                TextAlign = Alignment.MiddleLeft,
                BBCodeEnabled = true
            };
            Titlebar.CloseButton.MouseClick += (s, a) => Actions.Add(new Fade(0, 250)).Finished += w => { Close(); };
            Titlebar.MouseDown += (s, a) => StartDrag();
            Titlebar.MouseUp += (s, a) => StopDrag();
            Controls.Add(Titlebar);
        }

        public override void Show(Desktop target)
        {
            Opacity = 0;
            base.Show(target);
            Actions.Add(new Fade(1, 250));
        }
    }
}
