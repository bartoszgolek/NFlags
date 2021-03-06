﻿using System.Collections.Generic;
using NFlags.OptionReaders;
using NFlags.Utils;

namespace NFlags
{
    internal abstract class OptionReader
    {
        private static readonly Dictionary<OptionValueMode, OptionReader> Readers = new Dictionary<OptionValueMode, OptionReader>
        {
            { OptionValueMode.NextArgument, new ArgSeparatorOptionReader() },
            { OptionValueMode.AfterEqual, new EqualityOptionReader() }
            
        };
        
        public static OptionReader GetReader(OptionValueMode optionValueMode)
        {
            return Readers[optionValueMode];
        }

        public abstract string ReadValue(ArrayReader<string> args, string arg);
    }
}