using System.Collections.Generic;
using NFlags.OptionReaders;
using NFlags.Utils;

namespace NFlags
{
    internal abstract class OptionReader
    {
        private static readonly Dictionary<OptionSeparator, OptionReader> Readers = new Dictionary<OptionSeparator, OptionReader>
        {
            { OptionSeparator.ArgSeparator, new ArgSeparatorOptionReder() },
            { OptionSeparator.Equality, new EqualityOptionReder() },
            
        };
        
        public static OptionReader GetReader(OptionSeparator optionSeparator)
        {
            return Readers[optionSeparator];
        }

        public abstract string ReadValue(Shifter<string> args, string arg);
    }
}