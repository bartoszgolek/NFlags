using System;
using NFlags.TypeConverters;

namespace NFlags.Arguments
{
    /// <summary>
    /// Base class for all arguments
    /// </summary>
    public abstract class Argument
    {
        /// <summary>
        /// Name for help printing
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Description for help printing
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// Describes type of argument value
        /// </summary>
        public Type ValueType { get; internal set; }

        /// <summary>
        /// Converter used to parse argument value
        /// </summary>
        public IArgumentConverter Converter { get; internal set; }
    }
}