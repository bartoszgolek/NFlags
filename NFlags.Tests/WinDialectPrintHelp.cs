namespace NFlags.Tests
{
    public class WinDialectPrintHelp  : DialectPrintHelp
    {
        private const string ShortPrefix = "/";
        private const string LongPrefix = "/";
        private const string OptionValueSeparator = "=";

        public WinDialectPrintHelp()
            :base(Dialect.Win, ShortPrefix, LongPrefix, OptionValueSeparator)
        {
        }
    }
}