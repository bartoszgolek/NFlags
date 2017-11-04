using NFlags.Arguments;

namespace NFlags.OptionFormatters
{
    internal class EqualityOptionFormatter : OptionFormatter
    {
        private const char EqualitySign = '=';

        private readonly Dialect _dialect;

        public EqualityOptionFormatter(Dialect dialect)
        {
            _dialect = dialect;
        }

        public override string FormatName(Option option)
        {
            return _dialect.Prefix + option.Name + EqualitySign + "<" + option.Name + ">";
        }

        public override string FormatAbr(Option option)
        {
            return _dialect.AbrPrefix + option.Abr + EqualitySign + "<" + option.Name + ">";
        }
    }
}