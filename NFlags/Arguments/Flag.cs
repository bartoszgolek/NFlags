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

        /// <inheritdoc />
        public override bool RequireValue => false;
    }
}