namespace Ormeli.Core.Patterns
{
    public class Builder<T, C> where T : new() where C : Builder<T, C>, new()
    {
        protected T template ;

        public static implicit operator T(Builder<T, C> b)
        {
            return b.Get();
        }

        public static C Create()
        {
            return new C()
            {
                template = new T()
            };
        }

        public void CreateNew()
        {
            template = new T();
        }

        public virtual T Get()
        {
            return template;
        }
    }
}
