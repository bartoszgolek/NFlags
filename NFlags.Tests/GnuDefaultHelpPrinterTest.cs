namespace NFlags.Tests
{
    public class GnuDefaultHelpPrinterTest : DefaultHelpPrinterTest
    {
        private const string ShortPrefix = "-";
        private const string LongPrefix = "--";
        private const string OptionValueSeparator = " ";

        public GnuDefaultHelpPrinterTest()
            :base(Dialect.Gnu, ShortPrefix, LongPrefix, OptionValueSeparator)
        {
        }
    }
}