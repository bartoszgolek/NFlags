namespace NFlags.Tests
{
    public class WinDialectPrintHelp  : DialectPrintHelp
    {
        private const string SHORT_PREFIX = "/";
        private const string LONG_PREFIX = "/";
        private const string OPTION_VALUE_SEPARATOR = "=";

        public WinDialectPrintHelp()
            :base(Dialect.Win, SHORT_PREFIX, LONG_PREFIX, OPTION_VALUE_SEPARATOR)
        {
        }
    }
}