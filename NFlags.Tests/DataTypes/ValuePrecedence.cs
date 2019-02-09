using NFlags.GenericCommandExtension;

namespace NFlags.Tests.DataTypes
{
    public class ValuePrecedence
    {
        [Option(Name = "option", DefaultValue = "def_o", EnvironmentVariable = "NFLAG_TEST_OPTION_ENV", ConfigPath = "OPTION")]
        public string Option;

        [Flag(Name = "flag", DefaultValue = true, EnvironmentVariable = "NFLAG_TEST_FLAG_ENV", ConfigPath = "FLAG")]
        public bool Flag;

        [Parameter(Name = "parameter", DefaultValue = "def_p", EnvironmentVariable = "NFLAG_TEST_PARAM_ENV", ConfigPath = "PARAM")]
        public string Parameter { get; set; }
    }
}