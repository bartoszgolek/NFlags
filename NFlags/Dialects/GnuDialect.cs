namespace NFlags.Dialects
{
    /// <inheritdoc />
    /// <summary>
    /// Predefined dialect following Gnu standards.
    /// </summary>
    public class GnuDialect : Dialect
    {   
        /// <inheritdoc />
        public override string Prefix => "--";

        /// <inheritdoc />
        public override string AbrPrefix => "-";

        /// <inheritdoc />
        public override OptionValueMode OptionValueMode => OptionValueMode.NextArgument;
    }
}