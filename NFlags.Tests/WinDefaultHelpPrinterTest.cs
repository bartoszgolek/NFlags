namespace NFlags.Tests
{
    public class WinDefaultHelpPrinterTest  : DefaultHelpPrinterTest
    {
        private const string ShortPrefix = "/";
        private const string LongPrefix = "/";
        private const string OptionValueSeparator = "=";

        public WinDefaultHelpPrinterTest()
            :base(Dialect.Win, ShortPrefix, LongPrefix, OptionValueSeparator)
        {
        }
    }
}