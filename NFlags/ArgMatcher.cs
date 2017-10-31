using System;
using System.Collections.Generic;
using NFlags.ArgMatchers;

namespace NFlags
{
    internal abstract class ArgMatcher
    {
        private static readonly Dictionary<OptionSeparator, Func<Dialect, ArgMatcher>> Matchers =
            new Dictionary<OptionSeparator, Func<Dialect, ArgMatcher>>
            {
                {OptionSeparator.ArgSeparator, dialect => new ArgSeparatorMatcher(dialect)},
                {OptionSeparator.Equality, dialect => new EqualityMatcher(dialect)},
            };

        protected readonly Dialect Dialect;

        protected ArgMatcher(Dialect dialect)
        {
            this.Dialect = dialect;
        }

        public static ArgMatcher GetMatcher(Dialect dialect)
        {
            return Matchers[dialect.OptionSeparator](dialect);
        }

        public abstract bool IsOptionMatching(Option option, string arg);

        public bool IsFlagMatching(Flag flag, string arg)
        {
            return arg == Dialect.Prefix + flag.Name ||
                   flag.Abr != null && arg == Dialect.AbrPrefix + flag.Abr;
        }
    }
}