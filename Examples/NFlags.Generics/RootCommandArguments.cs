namespace NFlags.Generics
{
    public class RootCommandArguments
    {
        [Option("option", "o", "option description", 3)]
        public int Option;

        [Flag("flag", "f", "flag description", true)]
        public bool Flag;

        [Parameter("parameter", "parameter description", 1.1)]
        public double Parameter;

        [ParameterSeries("parameterSeries", "parameter series description")]
        public int[] ParameterSeries;
    }
}