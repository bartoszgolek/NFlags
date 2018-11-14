using NFlags.GenericCommandExtension;

namespace NFlags.Tests.DataTypes
{
    public class ArgumentsType
    {
        [Option("option1", "option desc", 1)]
        public int Option1;

        [Option("option2", "o2", "option2 desc", "asd")]
        public string Option2 { get; set; }

        [Flag("flag1", "flag desc", true)]
        public bool Flag1;

        [Flag("flag2", "f2", "flag2 desc", false)]
        public bool Flag2 { get; set; }

        [Parameter("parameter1", "parameter desc", 1.1)]
        public double Parameter1;

        [Parameter("parameter2", "parameter2 desc", 1)]
        public int Parameter2 { get; set; }

            [ParameterSeries("parameterSeries", "parameter series desc")]
        public int[] ParameterSeries { get; set; }
    }
}