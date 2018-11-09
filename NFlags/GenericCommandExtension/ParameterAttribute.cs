using System;

namespace NFlags.GenericCommandExtension
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ParameterAttribute : Attribute
    {
        public ParameterAttribute(string name, string description, bool defaultValue)
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