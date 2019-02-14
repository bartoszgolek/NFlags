using System;

namespace NFlags.Tests.DataTypes
{
    public class LazyEnvironmentArgumentsType
    {
        [Option(Name = "option", DefaultValue = "def_o", EnvironmentVariable = "NFLAG_TEST_OPTION_ENV")]
        public Lazy<string> Option;

        [Flag(Name = "flag", DefaultValue = true, EnvironmentVariable = "NFLAG_TEST_FLAG_ENV")]
        public Lazy<bool> Flag;

        [Parameter(Name = "parameter", DefaultValue = "def_p", EnvironmentVariable = "NFLAG_TEST_PARAM_ENV")]
        public Lazy<string> Parameter;
    }
}