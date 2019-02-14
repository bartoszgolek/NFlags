using System;

namespace NFlags.Tests.DataTypes
{
    public class LazyConfigPathArgumentsType
    {
        [Option(Name = "option", DefaultValue = "def_o", ConfigPath = "config.option")]
        public Lazy<string> Option;

        [Flag(Name = "flag", DefaultValue = true, ConfigPath = "config.flag")]
        public Lazy<bool> Flag;

        [Parameter(Name = "parameter", DefaultValue = "def_p", ConfigPath = "config.parameter")]
        public Lazy<string> Parameter;
    }
}