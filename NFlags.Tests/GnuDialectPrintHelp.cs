namespace NFlags.Tests
{
    public class GnuDialectPrintHelp : DialectPrintHelp
    {
        private const string ShortPrefix = "-";
        private const string LongPrefix = "--";
        private const string OptionValueSeparator = " ";

        public GnuDialectPrintHelp()
            :base(Dialect.Gnu, ShortPrefix, LongPrefix, OptionValueSeparator)
        {
        }
    }
}