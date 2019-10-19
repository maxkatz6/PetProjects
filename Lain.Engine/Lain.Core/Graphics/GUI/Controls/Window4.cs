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
    /// sample window 4 - TabControl, TextAlign
    /// </summary>
    public class Window4 : MyWindow
    {
        public Window4()
        {
            Size = new Point(440, 340);
            Position = new Point(500, 400);
            Resizable = true;
            Titlebar.Text = "TabControl, TextAlign";

            TabControl tabcontrol = new TabControl();
            tabcontrol.ButtonFrame.Style = "item";
            tabcontrol.Dock = DockStyle.Fill;
            Controls.Add(tabcontrol);

            TabPage tabPage = new TabPage();
            //tabPage.Style = "frame";
            tabPage.Margin = new Margin(0, -1, 0, 0);
            tabPage.Button.Style = "button";
            tabPage.Button.Text = "page";
            tabPage.Button.Tooltip = "Click to change active tab";
            tabPage.Button.Margin = new Margin(0, 0, -1, 0);
            tabcontrol.TabPages.Add(tabPage);

            TextArea lbl = new TextArea();
            lbl.Dock = DockStyle.Fill;
            lbl.TextWrap = true;
            lbl.Text = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.\n At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";
            lbl.Style = "multiline";
            lbl.Parent = tabPage;
            /*
            for (int i = 0; i < 6; i++)
            {
                TabPage tabPage1 = new TabPage();
                tabPage1.Style = "frame";
                tabPage1.Margin = new Margin(0, -1, 0, 0);
                tabPage1.Button.Style = "button";
                tabPage1.Button.Text = "page" + i;
                tabPage1.Button.Tooltip = "Click to change active tab";
                tabPage1.Button.Margin = new Margin(0, 0, -1, 0);
                tabcontrol.TabPages.Add(tabPage1);

                Label lbl1 = new Label();
                lbl1.Dock = DockStyle.Fill;
                lbl1.TextWrap = true;
                lbl1.Text = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore [color=ff0088ff][url=testurl]click \r\n meh![/url][/color] et dolore magna aliquyam erat, sed diam voluptua.\r\n At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";
                lbl1.Style = "multiline";
                lbl1.BBCodeEnabled = true;
                lbl1.LinkClicked += s=> MessageBox.Show(new Point(300, 200), "Message Box", s, MessageBoxButtons.OKCancel, Desktop);
                lbl1.Parent = tabPage1;
            }*/
        }
    }
}
