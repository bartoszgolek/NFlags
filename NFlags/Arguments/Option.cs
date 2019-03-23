namespace NFlags.Arguments
{
    /// <inheritdoc />
    /// <summary>
    /// Represents registered application option.
    /// </summary>
    public class Option : PrefixedDefaultValueArgument
    {
        /// <inheritdoc />
        public override bool RequireValue => true;
    }
}