using NFlags.Dialects;

namespace NFlags
{
    /// <summary>
    /// Dialect defines how flags and options are prefixed and how option value follows option.
    /// </summary>
    public abstract class Dialect
    {
        /// <summary>
        /// Defines prefix for options and flags.
        /// </summary>
        public abstract string Prefix { get; }

        /// <summary>
        /// Defines prefix for options and flags abbreviations.
        /// </summary>
        public abstract string AbrPrefix { get; }
        
        /// <summary>
        /// Defines how value follows option label.
        /// </summary>
        public abstract OptionValueMode OptionValueMode { get; }

        /// <summary>
        /// Predefined dialect following Gnu standards.
        /// </summary>
        public static Dialect Gnu => new GnuDialect();

        /// <summary>
        /// Predefined dialect following MS Windows standards.
        /// </summary>
        public static Dialect Win => new WinDialect();
    }
}