namespace NFlags.Arguments
{
    /// <summary>
    /// Base class for arguments with default values
    /// </summary>
    public abstract class DefaultValueArgument<T> : Argument
    {
        /// <summary>
        /// Default value
        /// </summary>
        public T DefaultValue;
    }
}