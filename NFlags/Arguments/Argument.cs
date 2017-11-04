namespace NFlags.Arguments
{
    /// <summary>
    /// Base class for all arguments
    /// </summary>
    public abstract class Argument<T>
    {
        /// <summary>
        /// Name for help printing
        /// </summary>
        public string Name;

        /// <summary>
        /// Description for help printing
        /// </summary>
        public string Description;

        /// <summary>
        /// Default value
        /// </summary>
        public T DefaultValue;
    }
}