using NFlags.Arguments;

namespace NFlags.ArgMatchers
{
    internal class ArgSeparatorMatcher : ArgMatcher
    {
        public ArgSeparatorMatcher(Dialect dialect)
            :base(dialect)
        {
        }

        public override bool IsOptionMatching(Option option, string arg)
        {
            return arg == Dialect.Prefix + option.Name || 
                   arg == Dialect.AbrPrefix + option.Abr;
        }
    }
}