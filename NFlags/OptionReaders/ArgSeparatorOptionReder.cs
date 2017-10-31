using NFlags.Utils;

namespace NFlags.OptionReaders
{
    internal class ArgSeparatorOptionReder : OptionReader
    {
        public override string ReadValue(Shifter<string> args, string arg)
        {
            return args.Shift();
        }
    }
}