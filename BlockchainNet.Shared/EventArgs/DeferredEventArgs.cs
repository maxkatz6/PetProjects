namespace BlockchainNet.Shared.EventArgs
{
    using System;

    using Nito.AsyncEx;

    public class DeferredEventArgs : EventArgs
    {
        internal IDeferralSource? DeferralSource { get; set; }

        public IDisposable GetDeferral()
        {
            return DeferralSource?.GetDeferral() ?? default(EmptyDisposable);
        }

        private struct EmptyDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
