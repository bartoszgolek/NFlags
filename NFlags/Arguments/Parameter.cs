namespace NFlags.Arguments
{
    /// <inheritdoc />
    /// <summary>
    /// Represents registered application parameter.
    /// </summary>
    public class Parameter : DefaultValueArgument
    {
        /// <inheritdoc />
        public override bool RequireValue => true;
    }
}