using System;

namespace NFlags
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ParameterAttribute : Attribute
    {
        public ParameterAttribute(string name, string description, object defaultValue)
        {
            Name = name;
            Description = description;
            DefaultValue = defaultValue;
        }

        public string Name { get; }
        public string Description { get; }
        public object DefaultValue { get; }
    }
}