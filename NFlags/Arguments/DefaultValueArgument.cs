namespace NFlags.Arguments
{
    /// <inheritdoc />
    /// <summary>
    /// Base class for arguments with default values
    /// </summary>
    public abstract class DefaultValueArgument : Argument
    {
        /// <summary>
        /// Default value
        /// </summary>
        public object DefaultValue;
    }
}