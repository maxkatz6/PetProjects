﻿using Squid.Interfaces;
using Squid.Structs;
using Squid.Util;

namespace Squid.Controls
{
    /// <summary>
    /// A RadioButton
    /// </summary>
    [Toolbox]
    public class RadioButton : Control, ICheckable, IText
    {
        private string _text;
        private bool _checked;

        /// <summary>
        /// Raised when [checked changed].
        /// </summary>
        public event VoidEvent CheckedChanged;

        /// <summary>
        /// Gets the button.
        /// </summary>
        /// <value>The button.</value>
        public Button Button { get; private set; }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>The label.</value>
        public Label Label { get; private set; }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>The group.</value>
        public int Group { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return Label.Text; }
            set
            {
                _text = value;

                if (Label != null)
                    Label.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RadioButton"/> is checked.
        /// </summary>
        /// <value><c>true</c> if checked; otherwise, <c>false</c>.</value>
        public bool Checked
        {
            get { return Button.Checked; }
            set
            {
                _checked = value;

                if (Button != null)
                    Button.Checked = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButton"/> class.
        /// </summary>
        public RadioButton()
        {
            Size = new Point(100, 30);
            Style = "checkbox";

            MouseClick += RadioButton_MouseClick;

            Button = new Button();
            Button.Dock = DockStyle.Left;
            Button.Size = new Point(29, 30);
            Button.CheckOnClick = true;
            Button.CheckedChanged += Button_CheckedChanged;
            Button.BeforeCheckedChanged += Button_BeforeCheckedChanged;
            Button.Style = "checkboxButton";
            Button.NoEvents = true;
            Elements.Add(Button);

            Label = new Label();
            Label.Dock = DockStyle.Fill;
            Label.Style = "checkboxLabel";
            Label.NoEvents = true;
            Elements.Add(Label);
        }

        void RadioButton_MouseClick(Control sender, MouseEventArgs args)
        {
            Button.Click(args.Button);
        }

        void Button_BeforeCheckedChanged(Control sender, SquidEventArgs args)
        {
            if (Button.Checked && Parent != null)
            {
                bool valid = false;

                foreach (RadioButton btn in Parent.GetControls<RadioButton>())
                {
                    if (btn != this && btn.Group == Group)
                    {
                        if (btn.Checked)
                        {
                            valid = true;
                            break;
                        }
                    }
                }

                if (!valid) args.Cancel = true;
            }
        }

        protected override void OnStateChanged()
        {
            base.OnStateChanged();

            Button.State = State;
            Label.State = State;
        }

        void Button_CheckedChanged(Control sender)
        {
            if (Button.Checked && Parent != null)
            {
                foreach (RadioButton btn in Parent.GetControls<RadioButton>())
                {
                    if (btn != this && btn.Group == Group)
                        btn.Checked = false;
                }
            }

            if (CheckedChanged != null)
                CheckedChanged(this);
        }
    }
}
