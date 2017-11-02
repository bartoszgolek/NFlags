using System.Collections.Generic;

namespace NFlags
{
    public class CommandConfig
    {
        public CommandConfig(
            List<Flag> flags, 
            List<Parameter> parameters, 
            List<Option> options)
        {
            Flags = flags;
            Parameters = parameters;
            Options = options;
        }

        public List<Flag> Flags { get; }

        public List<Parameter> Parameters { get; }

        public List<Option> Options { get; }
    }
}