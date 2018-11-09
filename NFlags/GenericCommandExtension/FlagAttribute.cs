using System;

namespace NFlags.GenericCommandExtension
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FlagAttribute : Attribute
    {
        public FlagAttribute(string name, string description, bool defaultValue)
        {
            Name = name;
            Description = description;
            DefaultValue = defaultValue;
        }

        public string Name { get; }
        public string Description { get; }
        public bool DefaultValue { get; }
    }
}