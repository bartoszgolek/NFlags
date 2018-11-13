namespace NFlags.Dialects
{
    /// <inheritdoc />
    /// <summary>
    /// Predefined dialect following MS Windows standards.
    /// </summary>
    public class WinDialect : Dialect
    {
        /// <inheritdoc />
        public override string Prefix => "/";

        /// <inheritdoc />
        public override string AbrPrefix => "/";

        /// <inheritdoc />
        public override OptionValueMode OptionValueMode => OptionValueMode.AfterEqual;
    }
}