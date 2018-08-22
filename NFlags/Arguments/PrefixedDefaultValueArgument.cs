namespace NFlags.Arguments
{
    /// <summary>
    /// Base class for arguments with abreviation
    /// </summary>
    public abstract class PrefixedDefaultValueArgument<T> : DefaultValueArgument<T>
    {
        /// <summary>
        /// Abreviation name for help printing and shorthand usage.
        /// </summary>
        public string Abr;

        /// <summary>
        /// Defines if flag persist for subcommands
        /// </summary>
        public bool IsPersistent;
    }
}