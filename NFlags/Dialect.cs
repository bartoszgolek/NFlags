using NFlags.Dialects;

namespace NFlags
{
    public abstract class Dialect
    {
        public abstract string Prefix { get; }

        public abstract string AbrPrefix { get; }
        
        public abstract OptionSeparator OptionSeparator { get; }

        public static Dialect Gnu => new GnuDialect();

        public static Dialect Win => new WinDialect();
    }
}