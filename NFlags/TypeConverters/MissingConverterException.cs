using System;

namespace NFlags.TypeConverters
{
    /// <inheritdoc />
    /// <summary>
    /// Exception is thrown when converter not exists for particular type
    /// </summary>
    public class MissingConverterException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates new exception instance.
        /// </summary>
        /// <param name="type">Type for whom converter is missing.</param>
        public MissingConverterException(Type type)
            :base($"Missing parameter converter for type '{type}'")
        {
        }
    }
}