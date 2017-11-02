using System;

namespace NFlags
{
    /// <summary>
    /// Exception is throwed when there is more parameters provided than registered.
    /// </summary>
    public class TooManyParametersException : Exception
    {
        /// <summary>
        /// Creates new exception instance.
        /// </summary>
        /// <param name="value">Value of parameter without registration.</param>
        public TooManyParametersException(string value)
            :base(string.Format("Two many parameters. Can't handle {0} value.", value))
        {
        }
    } 
}