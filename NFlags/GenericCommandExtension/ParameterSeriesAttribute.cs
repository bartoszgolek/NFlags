using System;

namespace NFlags.GenericCommandExtension
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ParameterSeriesAttribute : Attribute
    {
        public ParameterSeriesAttribute(string name, string description, object defaultValue)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }
        public string Description { get; }
    }
}