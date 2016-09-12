using System;

namespace Lain.Xaml.Attributes
{
    public class DisplayAttribute : Attribute
    {
        public DisplayAttribute(string name, string desk = null)
        {
            Name = name;
            Description = desk ?? string.Empty;
        }

        public string Description { get; set; }
        public string Name { get; set; }
    }
}