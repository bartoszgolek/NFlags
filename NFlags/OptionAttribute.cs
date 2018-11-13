using System;

namespace NFlags
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class OptionAttribute : Attribute
    {
        public OptionAttribute(string name, string abr, string description, object defaultValue)
        {
            Name = name;
            Abr = abr;
            Description = description;
            DefaultValue = defaultValue;
        }
        
        public OptionAttribute(string name, string description, object defaultValue)
        {
            Name = name;
            Description = description;
            DefaultValue = defaultValue;
        }

        public string Name { get; }
        public string Abr { get; }
        public string Description { get; }
        public object DefaultValue { get; }
    }
}