using System;

namespace NFlags
{
    /// <inheritdoc />
    /// <summary>
    /// Exception is thrown if member with attribute parameter Series is not an array
    /// </summary>
    public class IncorrectParameterSeriesMemberTypeException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of IncorrectParameterSeriesMemberTypeException
        /// </summary>
        /// <param name="name">Member name</param>
        /// <param name="type">Member type</param>
        public IncorrectParameterSeriesMemberTypeException(string name, Type type)
            :base($"Property '{name}' is of type '{type}'. Array expected.")
        {
        }
    }
}