using NFlags.Arguments;

namespace NFlags.OptionFormatters
{
    internal class EqualityOptionFormatter : OptionFormatter
    {
        private const string EqualitySign = "=";

        private readonly Dialect _dialect;

        public EqualityOptionFormatter(Dialect dialect)
        {
            _dialect = dialect;
        }

        public override string FormatName(PrefixedDefaultValueArgument option)
        {
            var name = $"{_dialect.Prefix}{option.Name}";
            if (option.RequireValue)
                name += $"{EqualitySign}<{option.Name}>";
            
            return name;
        }

        public override string FormatAbr(PrefixedDefaultValueArgument option)
        {
            var name = $"{_dialect.AbrPrefix}{option.Abr}";
            if (option.RequireValue)
                name += $"{EqualitySign}<{option.Name}>";
            
            return name;
        }
    }
}