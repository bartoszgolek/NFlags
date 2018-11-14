using NFlags.Utils;

namespace NFlags.OptionReaders
{
    internal class ArgSeparatorOptionReader : OptionReader
    {
        public override string ReadValue(Shifter<string> args, string arg)
        {
            return args.Shift();
        }
    }
}