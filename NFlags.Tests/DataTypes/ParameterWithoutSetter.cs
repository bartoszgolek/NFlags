namespace NFlags.Tests.DataTypes
{
    public class ParameterWithoutSetter
    {
        [Parameter("", "", "")]
        public int Parameter { get; }      
    }
}