using System;

namespace NFlags
{
    /// <inheritdoc />
    /// <summary>
    /// Exception is thrown if member with attribute Flag is not of type boolean
    /// </summary>
    public class IncorrectFlagMemberTypeException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of IncorrectFlagTypeException
        /// </summary>
        /// <param name="name">Member name</param>
        /// <param name="type">Member type</param>
        public IncorrectFlagMemberTypeException(string name, Type type)
            :base($"Property '{name}' is of type '{type}'. Boolean expected.")
        {
        }
    }
}