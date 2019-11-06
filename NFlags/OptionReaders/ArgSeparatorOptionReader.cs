using NFlags.Utils;

namespace NFlags.OptionReaders
{
    internal class ArgSeparatorOptionReader : OptionReader
    {
        public override string ReadValue(ArrayReader<string> args, string arg)
        {
            return args.Read();
        }
    }
}