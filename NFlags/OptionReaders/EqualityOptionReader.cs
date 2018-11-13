using NFlags.Utils;

namespace NFlags.OptionReaders
{
    internal class EqualityOptionReader : OptionReader
    {
        private const char EqualitySign = '=';

        public override string ReadValue(Shifter<string> args, string arg)
        {
            return arg.Split(new[] {EqualitySign}, 2)[1];
        }
    }
}