using Squid;
using Squid.Controls;
using Squid.Structs;
using Squid.Util;

namespace Lain.Graphics.GUI.Controls
{
    public class MessageBox : Dialog
    {
        private readonly Frame _buttonFrame;

        private MessageBox(string title, string message)
        {
            Modal = true;
            Scissor = false;
            Padding = new Margin(7);

            var titleLabel = new Label
            {
                Size = new Point(100, 35),
                Dock = DockStyle.Top,
                Text = title,
                Cursor = CursorNames.Move,
                Style = "frame",
                Margin = new Margin(0, 0, 0, -1)
            };
            titleLabel.MouseDown += (s, a) => StartDrag();
            titleLabel.MouseUp += (s, a) => StopDrag();
            Controls.Add(titleLabel);

            _buttonFrame = new Frame
            {
                Size = new Point(100, 35),
                Dock = DockStyle.Bottom
            };
            Controls.Add(_buttonFrame);

            var messageLabel = new Label
            {
                Dock = DockStyle.Fill,
                TextWrap = true,
                Text = message,
                NoEvents = true
            };
            Controls.Add(messageLabel);
        }

        public static MessageBox Show(Point size, string title, string message, MessageBoxButtons buttons,
            Desktop target)
        {
            var box = new MessageBox(title, message)
            {
                Size = size,
                Position = (target.Size - size)/2
            };
            box.InitButtons(buttons);
            box.Show(target);
            return box;
        }

        private void InitButtons(MessageBoxButtons buttons)
        {
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    AddButton("OK", DialogResult.OK, 1);
                    break;
                case MessageBoxButtons.OKCancel:
                    AddButton("Cancel", DialogResult.Cancel, 2);
                    AddButton("OK", DialogResult.OK, 2);
                    break;
                case MessageBoxButtons.RetryCancel:
                    AddButton("Cancel", DialogResult.Cancel, 2);
                    AddButton("Retry", DialogResult.Retry, 2);
                    break;
                case MessageBoxButtons.YesNo:
                    AddButton("No", DialogResult.No, 2);
                    AddButton("Yes", DialogResult.Yes, 2);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    AddButton("No", DialogResult.No, 3);
                    AddButton("Cancel", DialogResult.Cancel, 3);
                    AddButton("Yes", DialogResult.Yes, 3);
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    AddButton("Retry", DialogResult.Retry, 3);
                    AddButton("Ignore", DialogResult.Ignore, 3);
                    AddButton("Abort", DialogResult.Abort, 3);
                    break;
            }
        }

        private void AddButton(string text, DialogResult result, int divide)
        {
            var button = new Button
            {
                Style = "button",
                Cursor = CursorNames.Link,
                Margin = new Margin(2),
                Size = new Point(Size.x/(divide + 1), 35),
                Text = text,
                Tag = result,
                Dock = DockStyle.Right
            };
            button.MouseClick += button_OnMouseClick;
            _buttonFrame.Controls.Add(button);
        }

    /*    public override void Show(Desktop target)
        {
            base.Show(target);
            Opacity = 0;

            Actions.Add(new Fade(1, 250));
        }*/

        private void button_OnMouseClick(Control sender, MouseEventArgs args)
        {
            if (args.Button > 0) return;

           // Actions.Add(new Fade(0, 250)).Finished += w =>
            {
                Result = (DialogResult) sender.Tag;
                Close();
            };
        }
    }

    public enum MessageBoxButtons
    {
        OK = 0,
        OKCancel = 1,
        AbortRetryIgnore = 2,
        YesNoCancel = 3,
        YesNo = 4,
        RetryCancel = 5,
    }
}