﻿using System;
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

        public abstract bool IsOptionMatching(PrefixedDefaultValueArgument option, string arg);
    }
}