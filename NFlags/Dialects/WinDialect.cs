namespace NFlags.Dialects
{
    public class WinDialect : Dialect
    {      
        public override string Prefix => "/";

        public override string AbrPrefix => "/";

        public override OptionSeparator OptionSeparator => OptionSeparator.Equality;
    }
}