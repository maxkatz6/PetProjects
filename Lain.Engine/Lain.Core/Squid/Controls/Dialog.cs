﻿using Squid.Structs;

namespace Squid.Controls
{
    /// <summary>
    /// A Dialog window
    /// </summary>
    public abstract class Dialog : Window
    {
        private DialogResult _result;

        /// <summary>
        /// Delegate DialogResultEventHandler
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="result">The result.</param>
        public delegate void DialogResultEventHandler(Dialog sender, DialogResult result);

        /// <summary>
        /// Raised when [on result].
        /// </summary>
        public event DialogResultEventHandler OnResult;

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        protected DialogResult Result
        {
            get
            {
                return _result;
            }
            set
            {
                if (_result == value) return;

                _result = value;

                if (OnResult != null)
                    OnResult(this, _result);
            }
        }

        public override void Show(Desktop target)
        {
            _result = DialogResult.None;
            base.Show(target);
        }
    }
}
