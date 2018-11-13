namespace NFlags.Arguments
{
    /// <inheritdoc />
    /// <summary>
    /// Represents registered application flag.
    /// </summary>
    public class Flag : PrefixedDefaultValueArgument
    {
        /// <summary>
        /// Create new instance of flag definition
        /// </summary>
        public Flag()
        {
            ValueType = typeof(bool);
        }

        /// <summary>
        /// Default value of flag
        /// </summary>
        /// <returns>Default value of flag</returns>
        public bool GetDefault()
        {
            return (bool)DefaultValue;
        }
    }
}