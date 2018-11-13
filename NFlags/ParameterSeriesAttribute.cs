using System;

namespace NFlags
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ParameterSeriesAttribute : Attribute
    {
        public ParameterSeriesAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }
        public string Description { get; }
    }
}