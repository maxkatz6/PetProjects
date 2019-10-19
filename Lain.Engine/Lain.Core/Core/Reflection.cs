using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lain.Core
{
    public static class Reflection
    {
       /* public static TReturnType GetStatic<TReturnType, TType>(string s) where TReturnType : new()
        {
            var field = typeof (TType).GetField(s, BindingFlags.Static | BindingFlags.Public);
            return field != null ? (TReturnType) field.GetValue(null) : new TReturnType();
        }

        public static TReturnType Get<TReturnType, TType>(TType t, string s) where TReturnType : new()
        {
            var field = typeof (TType).GetField(s, BindingFlags.Public);
            return field != null ? (TReturnType) field.GetValue(t) : new TReturnType();
        }

        public static Dictionary<string, object> GetProperties<TType>(TType obj,
            BindingFlags bindFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            return typeof (TType).GetProperties(bindFlags).ToDictionary(prop => prop.Name, prop => prop.GetValue(obj) ?? "null");
        }*/
    }
}