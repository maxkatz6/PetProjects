using System;
using System.Reflection;
using Lain.Xaml.Attributes;

namespace Lain.Xaml.Helpers
{
    public static class EnumHelper
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue?.GetType().GetTypeInfo()
                .GetDeclaredField(enumValue.ToString())
                .GetCustomAttribute<DisplayAttribute>()?.Name;
        }
    }
}
