using System;

namespace NFlags.TypeConverters
{
    /// <summary>
    /// Exception is throwed when converter not exists for particular type
    /// </summary>
    public class MissingConverterException : Exception
    {
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