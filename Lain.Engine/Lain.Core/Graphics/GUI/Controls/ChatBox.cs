using System;
using Squid;
using Squid.Controls;
using Squid.Structs;

namespace Lain.Graphics.GUI.Controls
{
    public class ChatBox : Control
    {
        public TextBox Textbox { get; private set; }
        public Label Output { get; private set; }
        public ScrollBar Scrollbar { get; private set; }
        public Frame ClipFrame { get; private set; }

        public ChatBox()
        {
            Size = new Point(100, 100);

            Textbox = new TextBox
            {
                Size = new Point(100, 35),
                Dock = DockStyle.Bottom
            };
            Textbox.TextCommit += Input_OnTextCommit;
            Elements.Add(Textbox);

            Scrollbar = new ScrollBar
            {
                Dock = DockStyle.Right,
                Size = new Point(25, 25)
            };
            Elements.Add(Scrollbar);

            ClipFrame = new Frame
            {
                Dock = DockStyle.Fill,
                Scissor = true,
                Margin = new Margin(0, 0, 0, 14)
            };
            Elements.Add(ClipFrame);

            Output = new Label
            {
                BBCodeEnabled = true,
                TextWrap = true,
                AutoSize = AutoSize.Vertical,
                Text =
                    "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.",
                Style = "multiline",
                Dock = DockStyle.FillX
            };
            ClipFrame.Controls.Add(Output);

            MouseWheel += ChatBox_MouseWheel;
        }

        void ChatBox_MouseWheel(Control sender, MouseEventArgs args)
        {
            Scrollbar.Scroll(Gui.MouseScroll);
            args.Cancel = true;
        }

        void Input_OnTextCommit(object sender, EventArgs e)
        {
            Append(Textbox.Text); // try to append the text
            Textbox.Text = string.Empty;
            Textbox.Focus();
        }

        protected override void OnUpdate()
        {
            // force the width to be that of its parent
            //Output.Size = new Point(Frame.Size.X, Output.Size.Y);

            // move the label up/down using the scrollbar value
            if (Output.Size.y < ClipFrame.Size.y) // no need to scroll
            {
                Scrollbar.Visible = false; // hide scrollbar
                Output.Position = new Point(0, ClipFrame.Size.y - Output.Size.y); // set fixed position
            }
            else
            {
                Scrollbar.Scale = Math.Min(1, (float)ClipFrame.Size.y / (float)Output.Size.y);
                Scrollbar.Visible = true; // show scrollbar
                Output.Position = new Point(0, (int)((ClipFrame.Size.y - Output.Size.y) * Scrollbar.EasedValue));
            }
        }

        public void Append(string text)
        {
            // check for null/empty
            if (string.IsNullOrEmpty(text))
                return;

            // return if only whitespaces were entered
            if (text.Trim().Length == 0)
                return;

            string prefix = ""; // "[Username]: ";

            if (string.IsNullOrEmpty(Output.Text))
                Output.Text = prefix + text;
            else
                Output.Text += Environment.NewLine + prefix + text;

            Scrollbar.Value = 1; // scroll down
        }
    }
}
