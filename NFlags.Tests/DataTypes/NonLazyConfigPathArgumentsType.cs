namespace NFlags.Tests.DataTypes
{
    public class NonLazyConfigPathArgumentsType
    {
        [Option(Name = "option", DefaultValue = "def_o", ConfigPath = "config.option")]
        public string Option;

        [Flag(Name = "flag", DefaultValue = true, ConfigPath = "config.flag")]
        public bool Flag;

        [Parameter(Name = "parameter", DefaultValue = "def_p", ConfigPath = "config.parameter")]
        public string Parameter;
    }
}