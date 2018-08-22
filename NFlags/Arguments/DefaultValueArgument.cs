namespace NFlags.Arguments
{
    /// <summary>
    /// Base class for all arguments
    /// </summary>
    public abstract class DefaultValueArgument<T> : Argument
    {
        /// <summary>
        /// Default value
        /// </summary>
        public T DefaultValue;
    }
}