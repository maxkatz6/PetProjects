using System;
using Newtonsoft.Json;
using Squid.Structs;
using Squid.Util;

namespace Squid.Skinning
{
    /// <summary>
    /// A ControlStyle. This is a set of Styles.
    /// There is one Style per ControlState.
    /// </summary>
    public sealed class ControlStyle
    {
        /// <summary>
        /// Gets or sets the styles.
        /// </summary>
        /// <value>The styles.</value>
        [Hidden]
        public StyleCollection Styles { get; set; }

        /// <summary>
        /// user data
        /// </summary>
        /// <value>The tag.</value>
        [Hidden, JsonIgnore]
        public object Tag { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlStyle"/> class.
        /// </summary>
        public ControlStyle()
        {
            Styles = new StyleCollection();

            foreach (ControlState state in Enum.GetValues(typeof(ControlState)))
                Styles.Add(state, new Style());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlStyle"/> class.
        /// </summary>
        /// <param name="style">The style.</param>
        public ControlStyle(ControlStyle style)
        {
            Styles = new StyleCollection();

            foreach (ControlState state in Enum.GetValues(typeof(ControlState)))
                Styles.Add(state, new Style(style.Styles[state]));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlStyle"/> class.
        /// </summary>
        /// <param name="style">The style.</param>
        public ControlStyle(Style style)
        {
            Styles = new StyleCollection();

            foreach (ControlState state in Enum.GetValues(typeof(ControlState)))
                Styles.Add(state, new Style(style));
        }

        /// <summary>
        /// Copies this instance.
        /// </summary>
        /// <returns>ControlStyle.</returns>
        public ControlStyle Copy()
        {
            return new ControlStyle(this);
        }

        /// <summary>
        /// Pastes the specified style.
        /// </summary>
        /// <param name="style">The style.</param>
        public void Paste(ControlStyle style)
        {
            Styles = new StyleCollection();

            foreach (ControlState state in style.Styles.Keys)
                Styles.Add(state, new Style(style.Styles[state]));
        }

        /// <summary>
        /// Pastes the specified style.
        /// </summary>
        /// <param name="style">The style.</param>
        public void Paste(Style style)
        {
            Styles = new StyleCollection();

            foreach (ControlState state in Enum.GetValues(typeof(ControlState)))
                Styles.Add(state, new Style(style));
        }

        /// <summary>
        /// Gets or sets the default.
        /// </summary>
        /// <value>The default.</value>
        [JsonIgnore]
        public Style Default
        {
            get { return Styles[ControlState.Default]; }
            set { Styles[ControlState.Default] = value; }
        }

        /// <summary>
        /// Gets or sets the hot.
        /// </summary>
        /// <value>The hot.</value>
        [JsonIgnore]
        public Style Hot
        {
            get { return Styles[ControlState.Hot]; }
            set { Styles[ControlState.Hot] = value; }
        }

        /// <summary>
        /// Gets or sets the pressed.
        /// </summary>
        /// <value>The pressed.</value>
        [JsonIgnore]
        public Style Pressed
        {
            get { return Styles[ControlState.Pressed]; }
            set { Styles[ControlState.Pressed] = value; }
        }

        /// <summary>
        /// Gets or sets the disabled.
        /// </summary>
        /// <value>The disabled.</value>
        [JsonIgnore]
        public Style Disabled
        {
            get { return Styles[ControlState.Disabled]; }
            set { Styles[ControlState.Disabled] = value; }
        }

        /// <summary>
        /// Gets or sets the focused.
        /// </summary>
        /// <value>The focused.</value>
        [JsonIgnore]
        public Style Focused
        {
            get { return Styles[ControlState.Focused]; }
            set { Styles[ControlState.Focused] = value; }
        }

        /// <summary>
        /// Gets or sets the checked.
        /// </summary>
        /// <value>The checked.</value>
        [JsonIgnore]
        public Style Checked
        {
            get { return Styles[ControlState.Checked]; }
            set { Styles[ControlState.Checked] = value; }
        }

        /// <summary>
        /// Gets or sets the checked hot.
        /// </summary>
        /// <value>The checked hot.</value>
        [JsonIgnore]
        public Style CheckedHot
        {
            get { return Styles[ControlState.CheckedHot]; }
            set { Styles[ControlState.CheckedHot] = value; }
        }

        /// <summary>
        /// Gets or sets the checked pressed.
        /// </summary>
        /// <value>The checked pressed.</value>
        [JsonIgnore]
        public Style CheckedPressed
        {
            get { return Styles[ControlState.CheckedPressed]; }
            set { Styles[ControlState.CheckedPressed] = value; }
        }

        /// <summary>
        /// Gets or sets the checked disabled.
        /// </summary>
        /// <value>The checked disabled.</value>
        [JsonIgnore]
        public Style CheckedDisabled
        {
            get { return Styles[ControlState.CheckedDisabled]; }
            set { Styles[ControlState.CheckedDisabled] = value; }
        }

        /// <summary>
        /// Gets or sets the checked focused.
        /// </summary>
        /// <value>The checked focused.</value>
        [JsonIgnore]
        public Style CheckedFocused
        {
            get { return Styles[ControlState.CheckedFocused]; }
            set { Styles[ControlState.CheckedFocused] = value; }
        }

        /// <summary>
        /// Gets or sets the selected.
        /// </summary>
        /// <value>The selected.</value>
        [JsonIgnore]
        public Style Selected
        {
            get { return Styles[ControlState.Selected]; }
            set { Styles[ControlState.Selected] = value; }
        }

        /// <summary>
        /// Gets or sets the selected hot.
        /// </summary>
        /// <value>The selected hot.</value>
        [JsonIgnore]
        public Style SelectedHot
        {
            get { return Styles[ControlState.SelectedHot]; }
            set { Styles[ControlState.SelectedHot] = value; }
        }

        /// <summary>
        /// Gets or sets the selected pressed.
        /// </summary>
        /// <value>The selected pressed.</value>
        [JsonIgnore]
        public Style SelectedPressed
        {
            get { return Styles[ControlState.SelectedPressed]; }
            set { Styles[ControlState.SelectedPressed] = value; }
        }

        /// <summary>
        /// Gets or sets the selected disabled.
        /// </summary>
        /// <value>The selected disabled.</value>
        [JsonIgnore]
        public Style SelectedDisabled
        {
            get { return Styles[ControlState.SelectedDisabled]; }
            set { Styles[ControlState.SelectedDisabled] = value; }
        }

        /// <summary>
        /// Gets or sets the selected focused.
        /// </summary>
        /// <value>The selected focused.</value>
        [JsonIgnore]
        public Style SelectedFocused
        {
            get { return Styles[ControlState.SelectedFocused]; }
            set { Styles[ControlState.SelectedFocused] = value; }
        }

        /// <summary>
        /// color to tint the texture (argb)
        /// </summary>
        /// <value>The tint.</value>
        [JsonIgnore]
        public int Tint
        {
            set
            {
                foreach (Style state in Styles.Values)
                    state.Tint = value;
            }
        }

        /// <summary>
        /// color for any text to be drawn (argb)
        /// </summary>
        /// <value>The color of the text.</value>
        [JsonIgnore]
        public int TextColor
        {
            set
            {
                foreach (Style state in Styles.Values)
                    state.TextColor = value;
            }
        }

        /// <summary>
        /// background color (argb)
        /// </summary>
        /// <value>The color of the back.</value>
        [JsonIgnore]
        public int BackColor
        {
            set
            {
                foreach (Style state in Styles.Values)
                    state.BackColor = value;
            }
        }

        /// <summary>
        /// opacity (0-1)
        /// </summary>
        /// <value>The opacity.</value>
        [JsonIgnore]
        public float Opacity
        {
            set
            {
                foreach (Style state in Styles.Values)
                    state.Opacity = value;
            }
        }

        /// <summary>
        /// name of the font to use for text
        /// </summary>
        /// <value>The font.</value>
        [JsonIgnore]
        public string Font
        {
            set
            {
                foreach (Style state in Styles.Values)
                    state.Font = value;
            }
        }

        /// <summary>
        /// name of the texture to draw
        /// </summary>
        /// <value>The texture.</value>
        [JsonIgnore]
        public string Texture
        {
            set
            {
                foreach (Style state in Styles.Values)
                    state.Texture = value;
            }
        }

        /// <summary>
        /// texture tiling mode
        /// </summary>
        /// <value>The tiling.</value>
        [JsonIgnore]
        public TextureMode Tiling
        {
            set
            {
                foreach (Style state in Styles.Values)
                    state.Tiling = value;
            }
        }

        /// <summary>
        /// source rectangle of the texture expressed in pixels
        /// </summary>
        /// <value>The texture rect.</value>
        [JsonIgnore]
        public Rectangle TextureRect
        {
            set
            {
                foreach (Style state in Styles.Values)
                    state.TextureRect = value;
            }
        }

        /// <summary>
        /// text padding (distance to control borders)
        /// </summary>
        /// <value>The text padding.</value>
        [JsonIgnore]
        public Margin TextPadding
        {
            set
            {
                foreach (Style state in Styles.Values)
                    state.TextPadding = value;
            }
        }

        /// <summary>
        /// text alignment
        /// </summary>
        /// <value>The text align.</value>
        [JsonIgnore]
        public Alignment TextAlign
        {
            set
            {
                foreach (Style state in Styles.Values)
                    state.TextAlign = value;
            }
        }

        /// <summary>
        /// describes the 9sclice texture regions expressed as margin
        /// </summary>
        /// <value>The grid.</value>
        [JsonIgnore]
        public Margin Grid
        {
            set
            {
                foreach (Style state in Styles.Values)
                    state.Grid = value;
            }
        }
    }
}