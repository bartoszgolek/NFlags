using NFlags.Arguments;

namespace NFlags.ArgMatchers
{
    internal class EqualityMatcher : ArgMatcher
    {
        public EqualityMatcher(Dialect dialect)
            :base(dialect)
        {
        }

        public override bool IsOptionMatching(PrefixedDefaultValueArgument option, string arg)
        {
            if (option.RequireValue)
            {
                return arg.StartsWith($"{Dialect.Prefix}{option.Name}=") ||
                       option.Abr != null && arg.StartsWith($"{Dialect.AbrPrefix}{option.Abr}=");
            }
            
            return arg == $"{Dialect.Prefix}{option.Name}" || option.Abr != null && arg == $"{Dialect.AbrPrefix}{option.Abr}";
        }
    }
}