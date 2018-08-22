namespace NFlags.Tests
{
    public class GnuDialectPrintHelp : DialectPrintHelp
    {
        private const string SHORT_PREFIX = "-";
        private const string LONG_PREFIX = "--";
        private const string OPTION_VALUE_SEPARATOR = " ";

        public GnuDialectPrintHelp()
            :base(Dialect.Gnu, SHORT_PREFIX, LONG_PREFIX, OPTION_VALUE_SEPARATOR)
        {
        }
    }
}