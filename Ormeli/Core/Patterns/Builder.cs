namespace Ormeli.Core.Patterns
{
	public class Builder<T, TC> where T : new() where TC : Builder<T, TC>, new()
	{
		protected T Template;

		public static implicit operator T(Builder<T, TC> b)
		{
			return b.Get();
		}

		public static TC Create()
		{
			return new TC
			{
				Template = new T()
			};
		}

		public void CreateNew()
		{
			Template = new T();
		}

		public virtual T Get()
		{
			return Template;
		}
	}
}