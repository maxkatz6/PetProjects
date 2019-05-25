namespace BlockchainNet.Shared.EventArgs
{
    using System;
    using System.Threading.Tasks;

    using Nito.AsyncEx;

    public static class DeferredEventArgsExtension
    {
        public static Task InvokeAsync<T>(this EventHandler<T>? eventHandler, object sender, T eventArgs)
            where T : DeferredEventArgs
        {
            if (eventHandler == null)
            {
                return Task.CompletedTask;
            }

            var manager = new DeferralManager();

            eventArgs.DeferralSource = manager.DeferralSource;
            eventHandler.Invoke(sender, eventArgs);
            return manager.WaitForDeferralsAsync();
        }
    }
}
