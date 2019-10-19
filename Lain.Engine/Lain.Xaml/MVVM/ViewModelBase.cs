namespace Lain.Xaml.MVVM
{
    public abstract class ViewModelBase : BindableBase
    {
        public bool ShowProgress
        {
            get { return Get<bool>(); }
            protected set { Set(value); }
        }
        protected NavigationService NavigationService { get; } = NavigationService.Instance;
    }
}