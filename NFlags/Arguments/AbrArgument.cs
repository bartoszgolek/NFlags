namespace NFlags.Arguments
{
    /// <summary>
    /// Base class for arguments with abreviation
    /// </summary>
    public abstract class AbrArgument<T> : Argument<T>
    {
        /// <summary>
        /// Abreviation name for help printing and shorthand usage.
        /// </summary>
        public string Abr;
    }
}