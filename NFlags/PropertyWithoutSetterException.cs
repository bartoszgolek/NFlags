using System;

namespace NFlags
{
    /// <inheritdoc />
    /// <summary>
    /// Exception is thrown if property with attribute Flag is not of type boolean
    /// </summary>
    public class PropertyWithoutSetterException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of PropertyWithoutSetterException
        /// </summary>
        /// <param name="name">Member name</param>
        public PropertyWithoutSetterException(string name)
            :base($"Property '{name}' has no public setter. Cannot be used.")
        {
        }
    }
}