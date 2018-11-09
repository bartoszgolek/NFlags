using NFlags.GenericCommandExtension;

namespace NFlags.Tests.DataTypes
{
    public class ArgumentsType
    {
        [Option("option1", "option desc", 1)]
        public int Option1;
    }
}