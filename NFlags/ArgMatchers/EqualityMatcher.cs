using NFlags.Arguments;

namespace NFlags.ArgMatchers
{
    internal class EqualityMatcher : ArgMatcher
    {
        public EqualityMatcher(Dialect dialect)
            :base(dialect)
        {
        }

        public override bool IsOptionMatching(Option option, string arg)
        {
            return arg.StartsWith(Dialect.Prefix + option.Name + '=') || 
                   option.Abr != null && arg.StartsWith(Dialect.AbrPrefix + option.Abr + '=');
        }
    }
}