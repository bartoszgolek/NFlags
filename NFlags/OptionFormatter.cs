using System;
using System.Collections.Generic;
using NFlags.OptionFormatters;

namespace NFlags
{
    internal abstract class OptionFormatter
    {
        private static readonly Dictionary<OptionSeparator, Func<Dialect, OptionFormatter>> Printers = new Dictionary<OptionSeparator, Func<Dialect, OptionFormatter>>
        {
            { OptionSeparator.ArgSeparator, dialect => new ArgSeparatorOptionFormatter(dialect) },
            { OptionSeparator.Equality, dialect => new EqualityOptionFormatter(dialect) },
            
        };
        
        public static OptionFormatter GetFormatter(Dialect dialect)
        {
            return Printers[dialect.OptionSeparator](dialect);
        }

        public abstract string FormatName(Option option);
        public abstract string FormatAbr(Option option);
    }
}