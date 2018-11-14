using System;
using System.Collections.Generic;
using NFlags.ArgMatchers;
using NFlags.Arguments;

namespace NFlags
{
    internal abstract class ArgMatcher
    {
        private static readonly Dictionary<OptionValueMode, Func<Dialect, ArgMatcher>> Matchers =
            new Dictionary<OptionValueMode, Func<Dialect, ArgMatcher>>
            {
                {OptionValueMode.NextArgument, dialect => new ArgSeparatorMatcher(dialect)},
                {OptionValueMode.AfterEqual, dialect => new EqualityMatcher(dialect)}
            };

        protected readonly Dialect Dialect;

        protected ArgMatcher(Dialect dialect)
        {
            Dialect = dialect;
        }

        public static ArgMatcher GetMatcher(Dialect dialect)
        {
            return Matchers[dialect.OptionValueMode](dialect);
        }

        public abstract bool IsOptionMatching(Option option, string arg);

        public bool IsFlagMatching(Flag flag, string arg)
        {
            return arg == GetPrefixedName(flag) ||
                   flag.Abr != null && arg == GetPrefixedAbbreviation(flag);
        }

        private string GetPrefixedAbbreviation(PrefixedDefaultValueArgument flag)
        {
            return $"{Dialect.AbrPrefix}{flag.Abr}";
        }

        private string GetPrefixedName(Argument flag)
        {
            return $"{Dialect.Prefix}{flag.Name}";
        }
    }
}