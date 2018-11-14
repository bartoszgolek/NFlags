namespace NFlags.Generics
{
    public class Command2Arguments
    {
        [Option("option2", "o2", "option description", 3)]
        public int Option { get; set; }

        [Flag("flag2", "f2", "flag description", true)]
        public bool Flag { get; set; }

        [Parameter("parameter2", "parameter description", 1.1)]
        public double Parameter { get; set; }

        [ParameterSeries("parameterSeries2", "parameter series description")]
        public int[] ParameterSeries { get; set; }
    }
}