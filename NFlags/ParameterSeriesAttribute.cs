using System;

namespace NFlags
{
    /// <inheritdoc />
    /// <summary>
    /// Mark field or property as parameter to register in command.
    /// </summary>
    /// <remarks>Must be an array. If property, then has to be settable.</remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ParameterSeriesAttribute : Attribute
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of parameter series
        /// </summary>
        public ParameterSeriesAttribute()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of parameter series
        /// </summary>
        /// <param name="name">Name of parameter series</param>
        /// <param name="description">Description of parameter series used in help</param>
        public ParameterSeriesAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Name of parameter series
        /// </summary>
        public string Name { get;  set; }

        /// <summary>
        /// Description of parameter series used in help
        /// </summary>
        public string Description { get;  set; }
    }
}