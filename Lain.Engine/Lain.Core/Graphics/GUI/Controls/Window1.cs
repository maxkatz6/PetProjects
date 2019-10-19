using Squid;
using Squid.Controls;
using Squid.Structs;
using Squid.Util;

namespace Lain.Graphics.GUI.Controls
{
    /// <summary>
    /// sample window 1 - Anchoring, DropDown, Modal Dialog
    /// </summary>
    public class Window1 : MyWindow
    {
        public Window1()
        {
            Size = new Point(440, 340);
            Position = new Point(40, 40);
            Titlebar.Text = "Anchoring, [color=FfFfFf00]DropDown, Modal Dialog[/color]";
            Resizable = true;

            var label1 = new Label
            {
                Text = "username:",
                Size = new Point(122, 35),
                Position = new Point(60, 100)
            };
            label1.MousePress += label1_OnMouseDown;
            Controls.Add(label1);

            var textbox1 = new TextBox
            {
                Name = "textbox",
                Text = "username",
                Size = new Point(222, 35),
                Position = new Point(180, 100),
                Style = "textbox",
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right
            };
            Controls.Add(textbox1);

            var label2 = new Label
            {
                Text = "password:",
                Size = new Point(122, 35),
                Position = new Point(60, 140)
            };
            Controls.Add(label2);

            var textbox2 = new TextBox
            {
                Name = "textbox",
                PasswordChar = '*',
                IsPassword = true,
                Text = "password",
                Size = new Point(222, 35),
                Position = new Point(180, 140),
                Style = "textbox",
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right
            };
            Controls.Add(textbox2);

            var button = new Button
            {
                Size = new Point(157, 35),
                Position = new Point(437 - 192, 346 - 52),
                Text = "login",
                Style = "button",
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Cursor = CursorNames.Link
            };
            button.MouseClick += button_OnMouseClick;
            Controls.Add(button);

            var combo = new DropDownList
            {
                Size = new Point(222, 35),
                Position = new Point(180, 180),
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Label = {Style = "comboLabel"},
                Button = {Style = "comboButton"},
                Listbox =
                {
                    Margin = new Margin(0, 6, 0, 0),
                    Style = "frame",
                    ClipFrame = {Margin = new Margin(8, 8, 8, 8)},
                    Scrollbar =
                    {
                        Margin = new Margin(0, 4, 4, 4),
                        Size = new Point(14, 10),
                        ButtonUp = {Style = "vscrollUp", Size = new Point(10, 20)},
                        ButtonDown = {Style = "vscrollUp", Size = new Point(10, 20)},
                        Slider =
                        {
                            Margin = new Margin(0, 2, 0, 2),
                            Style = "vscrollTrack",
                            Button = {Style = "vscrollButton"}
                        }
                    }
                }
            };

            Controls.Add(combo);

            for (int i = 0; i < 10; i++)
            {
                combo.Items.Add(new ListBoxItem
                {
                    Text = "listboxitem " + i,
                    Size = new Point(100, 35),
                    Margin = new Margin(0, 0, 0, 4),
                    Style = "item",
                    Selected = i == 3
                });
            }
            
            Controls.Add(new CheckBox
            {
                Size = new Point(157, 26),
                Position = new Point(180, 220),
                Text = "remember me",
                Button =
                {
                    Style = "checkBox",
                    Size = new Point(26,26),
                    Cursor = CursorNames.Link
                }
            });
        }

        void label1_OnMouseDown(Control sender, MouseEventArgs args)
        {
            sender.DoDragDrop(new Button
            {
                Size = new Point(157, 26),
                Text = "drag me",
                Position = sender.Location
            });
        }

        void button_OnMouseClick(Control sender, MouseEventArgs args)
        {
            MessageBox.Show(new Point(300, 200), "Message Box", "This is a modal Dialog.", MessageBoxButtons.OK, Desktop);
        }
    }
}
