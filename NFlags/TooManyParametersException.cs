using System;

namespace NFlags
{
    /// <inheritdoc />
    /// <summary>
    /// Exception is thrown when there is more parameters provided than registered.
    /// </summary>
    public class TooManyParametersException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates new exception instance.
        /// </summary>
        /// <param name="value">Value of parameter without registration.</param>
        public TooManyParametersException(string value)
            :base($"Two many parameters. Can't handle {value} value.")
        {
        }
    }
}