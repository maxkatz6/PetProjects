using System.Reflection;

namespace Ormeli.Core
{
    public static class Reflection
    {
        public static TReturnType GetStatic<TReturnType, TType>( string s) where TReturnType : new()
        {
            var field = typeof(TType).GetField(s, BindingFlags.Static | BindingFlags.Public);
            return field != null ? (TReturnType)field.GetValue(null) : new TReturnType();
        }

        public static TReturnType Get<TReturnType, TType>(TType t, string s) where TReturnType : new()
        {
            var field = typeof(TType).GetField(s, BindingFlags.Public);
            return field != null ? (TReturnType)field.GetValue(t) : new TReturnType();
        }
    }
}
