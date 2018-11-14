namespace NFlags.Tests.DataTypes
{
    public class OptionWithoutSetter
    {
        [Option("", "", "")]
        public int Option { get; }    
    }
}