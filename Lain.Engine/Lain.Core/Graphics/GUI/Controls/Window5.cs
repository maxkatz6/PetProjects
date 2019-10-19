using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lain.Graphics.GUI.Controls;
using Squid;
using Squid.Controls;
using Squid.Structs;

namespace SampleGui
{
    /// <summary>
    /// sample window 5 - Panel, TextBox
    /// </summary>
    public class Window5 : MyWindow
    {
        public Window5()
        {
            Size = new Point(440, 340);
            Position = new Point(960, 40);
            Resizable = true;
            Titlebar.Text = "Panel, TextBox";

            Panel panel = new Panel();
            panel.Style = "frame";
            panel.Dock = DockStyle.Fill;
            Controls.Add(panel);

            panel.ClipFrame.Margin = new Margin(8);
            panel.ClipFrame.Style = "textbox";

            panel.VScroll.Margin = new Margin(0, 8, 8, 8);
            panel.VScroll.Size = new Point(14, 10);
            panel.VScroll.Slider.Style = "vscrollTrack";
            panel.VScroll.Slider.Button.Style = "vscrollButton";
            panel.VScroll.ButtonUp.Style = "vscrollUp";
            panel.VScroll.ButtonUp.Size = new Point(10, 20);
            panel.VScroll.ButtonDown.Style = "vscrollUp";
            panel.VScroll.ButtonDown.Size = new Point(10, 20);
            panel.VScroll.Slider.Margin = new Margin(0, 2, 0, 2);

            panel.HScroll.Margin = new Margin(8, 0, 8, 8);
            panel.HScroll.Size = new Point(10, 14);
            panel.HScroll.Slider.Style = "hscrollTrack";
            panel.HScroll.Slider.Button.Style = "hscrollButton";
            panel.HScroll.ButtonUp.Style = "hscrollUp";
            panel.HScroll.ButtonUp.Size = new Point(20, 10);
            panel.HScroll.ButtonDown.Style = "hscrollUp";
            panel.HScroll.ButtonDown.Size = new Point(20, 10);
            panel.HScroll.Slider.Margin = new Margin(2, 0, 2, 0);

            for (int i = 0; i < 10; i++)
            {
                Label label = new Label();
                label.Text = "label control:";
                label.Size = new Point(100, 35);
                label.Position = new Point(10, 10 + 45 * i);
                panel.Content.Controls.Add(label);

                TextBox txt = new TextBox();
                txt.Text = "lorem ipsum";
                txt.Size = new Point(222, 35);
                txt.Position = new Point(110, 10 + 45 * i);
                txt.Style = "textbox";
                txt.AllowDrop = true;
                txt.TabIndex = 1 + i;
                txt.DragDrop += txt_OnDragDrop;
                txt.GotFocus += txt_OnGotFocus;
                panel.Content.Controls.Add(txt);
            }
        }

        void txt_OnGotFocus(Control sender)
        {
            TextBox txt = sender as TextBox;
            txt.Select(0, txt.Text.Length);
        }

        void txt_OnDragDrop(Control sender, DragDropEventArgs e)
        {
            if (e.Source is Label)
            {
                ((TextBox)sender).Text = ((Button)e.DraggedControl).Text;
            }
        }
    }
}
