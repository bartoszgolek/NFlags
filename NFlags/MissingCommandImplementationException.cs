using System;

namespace NFlags
{
    /// <inheritdoc />
    /// <summary>
    /// Exception thrown when trying to run command without execution delegate;
    /// </summary>
    public class MissingCommandImplementationException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Create new MissingCommandImplementationException
        /// </summary>
        public MissingCommandImplementationException()
            :base("Cannot execute command. Missing implementation.")
        {
        }
    }
}