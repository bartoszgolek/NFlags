namespace NFlags.OptionFormatters
{
    internal class ArgSeparatorOptionFormatter : OptionFormatter
    {
        private readonly Dialect _dialect;

        public ArgSeparatorOptionFormatter(Dialect dialect)
        {
            _dialect = dialect;
        }

        public override string FormatName(Option option)
        {
            return _dialect.Prefix + option.Name + " <" + option.Name + ">";
        }

        public override string FormatAbr(Option option)
        {
            return _dialect.AbrPrefix + option.Abr + " <" + option.Name + ">";
        }
    }
}