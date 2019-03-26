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
        public string Abr { get; internal set; }

        /// <summary>
        /// Defines if flag persist for sub commands
        /// </summary>
        public bool IsPersistent { get; internal set; }

        /// <summary>
        /// Defines grouping of arguments when printing help
        /// </summary>
        public string Group { get; internal set; }
    }
}