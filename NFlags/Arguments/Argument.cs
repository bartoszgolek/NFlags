namespace NFlags.Arguments
{
    /// <summary>
    /// Base class for all arguments
    /// </summary>
    public abstract class Argument
    {
        /// <summary>
        /// Name for help printing
        /// </summary>
        public string Name;

        /// <summary>
        /// Description for help printing
        /// </summary>
        public string Description;
    }
}