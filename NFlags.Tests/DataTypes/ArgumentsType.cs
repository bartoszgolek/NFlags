namespace NFlags.Tests.DataTypes
{
    public class ArgumentsType
    {
        [Option(Name = "option1", Description = "option desc", DefaultValue = 1, EnvironmentVariable = "NFLAG_TEST_OPTION1", ConfigPath = "Config:Path:Option1")]
        public int Option1;

        [Option("option2", "o2", "option2 desc", "asd")]
        public string Option2 { get; set; }

        [Flag(Name = "flag1", Description = "flag desc", DefaultValue = true, EnvironmentVariable = "NFLAG_TEST_FLAG1", ConfigPath = "Config:Path:Flag1")]
        public bool Flag1;

        [Flag("flag2", "f2", "flag2 desc", false)]
        public bool Flag2 { get; set; }

        [Parameter("parameter1", "parameter desc", 1.1)]
        public double Parameter1;

        [Parameter(Name = "parameter2", Description = "parameter2 desc", DefaultValue = 1, EnvironmentVariable = "NFLAG_TEST_PARAMETER2", ConfigPath = "Config:Path:Parameter2")]
        public int Parameter2 { get; set; }

        [ParameterSeries(Name = "parameterSeries", Description = "parameter series desc")]
        public int[] ParameterSeries { get; set; }
    }
}