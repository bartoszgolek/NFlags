using NFlags.Arguments;

namespace NFlags.OptionFormatters
{
    internal class ArgSeparatorOptionFormatter : OptionFormatter
    {
        private readonly Dialect _dialect;

        public ArgSeparatorOptionFormatter(Dialect dialect)
        {
            _dialect = dialect;
        }

        public override string FormatName(PrefixedDefaultValueArgument option)
        {
            var name = $"{_dialect.Prefix}{option.Name}";
            if (option.RequireValue)
                name += $" <{option.Name}>";
            
            return name;
        }

        public override string FormatAbr(PrefixedDefaultValueArgument option)
        {
            var name = $"{_dialect.AbrPrefix}{option.Abr}";
            if (option.RequireValue)
                name += $" <{option.Name}>";
            
            return name;
        }
    }
}