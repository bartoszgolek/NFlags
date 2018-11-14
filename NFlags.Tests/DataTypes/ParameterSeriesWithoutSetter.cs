namespace NFlags.Tests.DataTypes
{
    public class ParameterSeriesWithoutSetter
    {
        [ParameterSeries("", "")]
        public int[] ParameterSeries { get; }        
    }
}