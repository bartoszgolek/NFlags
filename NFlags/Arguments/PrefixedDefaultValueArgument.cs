namespace NFlags.Arguments
{
    /// <inheritdoc />
    /// <summary>
    /// Base class for arguments with abbreviation
    /// </summary>
    public abstract class PrefixedDefaultValueArgument : DefaultValueArgument
    {
        /// <summary>
        /// Abbreviation name for help printing and shorthand usage.
        /// </summary>
        public string Abr;

        /// <summary>
        /// Defines if flag persist for sub commands
        /// </summary>
        public bool IsPersistent;

        /// <summary>
        /// Describes type of argument value
        /// </summary>
        public string Target;
    }
}