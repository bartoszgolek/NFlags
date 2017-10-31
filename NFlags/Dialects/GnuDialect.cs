namespace NFlags.Dialects
{
    public class GnuDialect : Dialect
    {      
        public override string Prefix => "--";

        public override string AbrPrefix => "-";

        public override OptionSeparator OptionSeparator => OptionSeparator.ArgSeparator;
    }
}