using System;
using System.Collections.Generic;
using NFlags.Arguments;
using NFlags.OptionFormatters;

namespace NFlags
{
    internal abstract class OptionFormatter
    {
        private static readonly Dictionary<OptionValueMode, Func<Dialect, OptionFormatter>> Printers = new Dictionary<OptionValueMode, Func<Dialect, OptionFormatter>>
        {
            { OptionValueMode.NextArgument, dialect => new ArgSeparatorOptionFormatter(dialect) },
            { OptionValueMode.AfterEqual, dialect => new EqualityOptionFormatter(dialect) }
            
        };
        
        public static OptionFormatter GetFormatter(Dialect dialect)
        {
            return Printers[dialect.OptionValueMode](dialect);
        }

        public abstract string FormatName(PrefixedDefaultValueArgument option);
        public abstract string FormatAbr(PrefixedDefaultValueArgument option);
    }
}