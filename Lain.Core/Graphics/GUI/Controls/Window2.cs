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
    /// sample window 2 - SplitContainer, TreeView, ListBox
    /// </summary>
    
    public class Window2 : MyWindow
    {
        public Window2()
        {
            Size = new Point(440, 340);
            Position = new Point(500, 40);
            Titlebar.Text = "SplitContainer, TreeView, ListBox";
            Resizable = true;

            SplitContainer split = new SplitContainer {Dock = DockStyle.Fill};
            split.SplitButton.Style = "button";
            split.SplitFrame1.Size = new Point(10, 10);
            split.SplitFrame2.Size = new Point(30, 10);
            Controls.Add(split);

            ListBox listbox1 = new ListBox();
            listbox1.Margin = new Margin(2);
            listbox1.Dock = DockStyle.Fill;
            listbox1.Scrollbar.Size = new Point(14, 10);
            listbox1.Scrollbar.Slider.Style = "vscrollTrack";
            listbox1.Scrollbar.Slider.Button.Style = "vscrollButton";
            listbox1.Scrollbar.ButtonUp.Style = "vscrollUp";
            listbox1.Scrollbar.ButtonUp.Size = new Point(10, 20);
            listbox1.Scrollbar.ButtonDown.Style = "vscrollUp";
            listbox1.Scrollbar.ButtonDown.Size = new Point(10, 20);
            listbox1.Scrollbar.Slider.Margin = new Margin(0, 2, 0, 2);
            listbox1.Multiselect = true;
            listbox1.MaxSelected = 4;
            split.SplitFrame2.Controls.Add(listbox1);

            for (int i = 0; i < 30; i++)
            {
                ListBoxItem item = new ListBoxItem();
                item.Text = "listboxitem";
                item.Size = new Point(100, 26);
                item.Margin = new Margin(0, 0, 0, 1);
                item.Style = "label";
                item.Tooltip = "This is a multine tooltip.\nThe second line begins here.\n[color=ff00ee55]The third line is even colored.[/color]";
                item.MouseDrag += item_MouseDrag;
                listbox1.Items.Add(item);
            }

            TreeView treeview = new TreeView();
            treeview.Dock = DockStyle.Fill;
            treeview.Margin = new Margin(2);
            treeview.Scrollbar.Size = new Point(14, 10);
            treeview.Scrollbar.Slider.Style = "vscrollTrack";
            treeview.Scrollbar.Slider.Button.Style = "vscrollButton";
            treeview.Scrollbar.ButtonUp.Style = "vscrollUp";
            treeview.Scrollbar.ButtonUp.Size = new Point(10, 20);
            treeview.Scrollbar.ButtonDown.Style = "vscrollUp";
            treeview.Scrollbar.ButtonDown.Size = new Point(10, 20);
            treeview.Scrollbar.Slider.Margin = new Margin(0, 2, 0, 2);
            treeview.Indent = 10;
            split.SplitFrame1.Controls.Add(treeview);

            for (int i = 0; i < 30; i++)
            {
                TreeNodeLabel node = new TreeNodeLabel();
                node.Label.Text = "node level 1";
                node.Label.TextAlign = Alignment.MiddleLeft;
                node.Label.Style = "label";
                node.Button.Size = new Point(14, 14);
                node.Size = new Point(100, 26);
                node.Tooltip = node.Label.Text;
                node.Style = "label";
                treeview.Nodes.Add(node);

                for (int i2 = 0; i2 < 3; i2++)
                {
                    TreeNodeLabel sub1 = new TreeNodeLabel();
                    sub1.Size = new Point(100, 35);
                    sub1.Label.TextAlign = Alignment.MiddleLeft;
                    sub1.Label.Style = "label";
                    sub1.Button.Size = new Point(14, 14);
                    sub1.Size = new Point(100, 26);
                    sub1.Label.Text = "node level 2";
                    sub1.Tooltip = sub1.Label.Text;
                    sub1.Style = "label";
                    node.Nodes.Add(sub1);

                    for (int i3 = 0; i3 < 3; i3++)
                    {
                        TreeNodeLabel sub2 = new TreeNodeLabel();
                        sub2.Label.Text = "node level 3";
                        sub2.Label.TextAlign = Alignment.MiddleLeft;
                        sub2.Label.Style = "label";
                        sub2.Button.Size = new Point(14, 14);
                        sub2.Size = new Point(100, 26);
                        sub2.Tooltip = sub2.Label.Text;
                        sub2.Style = "label";
                        sub1.Nodes.Add(sub2);
                    }
                }
            }
        }

        void item_MouseDrag(Control sender, MouseEventArgs args)
        {

        }
    }
}
