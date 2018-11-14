namespace NFlags.Tests.DataTypes
{
    public class FlagWithoutSetter
    {
        [Flag("", "", true)]
        public bool Flag { get; }        
    }
}