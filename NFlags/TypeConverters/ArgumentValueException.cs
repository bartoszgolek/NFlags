using System;

namespace NFlags.TypeConverters
{
    /// <summary>
    /// Exception throw by converters if cannot convert given value to target type.
    /// </summary>
    /// <inheritdoc />
    public class ArgumentValueException : Exception
    {
        /// <summary>
        /// Creates new instance of ArgumentValueException   
        /// </summary>
        /// <param name="targetType">Target type</param>
        /// <param name="value">Value of argument.</param>
        /// <inheritdoc />
        public ArgumentValueException(Type targetType, string value)
            : base($"Cannot convert value '{value}' to type '{targetType}'")
        {
        }
    }
}