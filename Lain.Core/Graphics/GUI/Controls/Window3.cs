using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lain.Graphics.GUI.Controls;
using Squid;
using Squid.Structs;

namespace SampleGui
{
    /// <summary>
    /// sample window 3 - Custom Control (Inheritance)
    /// </summary>
    public class Window3 : MyWindow
    {
        public Window3()
        {
            Size = new Point(440, 340);
            Position = new Point(40, 400);
            Resizable = true;
            Titlebar.Text = "Custom Control (Inheritance)";

            ChatBox chatbox = new ChatBox();
            chatbox.Dock = DockStyle.Fill;
            Controls.Add(chatbox);

            chatbox.Style = "frame";
            chatbox.Textbox.Style = "textbox";
            chatbox.Textbox.Margin = new Margin(8, 0, 8, 8);
            chatbox.Output.Margin = new Margin(8, 8, 8, 8);
            //chatbox.Output.Style = "textbox";
            chatbox.Scrollbar.Margin = new Margin(0, 8, 8, 8);
            chatbox.Scrollbar.Size = new Point(14, 10);
            chatbox.Scrollbar.Slider.Style = "vscrollTrack";
            chatbox.Scrollbar.Slider.Button.Style = "vscrollButton";
            chatbox.Scrollbar.ButtonUp.Style = "vscrollUp";
            chatbox.Scrollbar.ButtonUp.Size = new Point(10, 20);
            chatbox.Scrollbar.ButtonDown.Style = "vscrollUp";
            chatbox.Scrollbar.ButtonDown.Size = new Point(10, 20);
            chatbox.Scrollbar.Slider.Margin = new Margin(0, 2, 0, 2);
        }
    }
}
