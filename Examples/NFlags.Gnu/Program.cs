using System;

namespace NFlags.Gnu
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            NFlags.Configure(configurator => configurator
                    .SetDescription("Application description")
                    .SetDialect(Dialect.Gnu)
                    .SetOutput(Console.WriteLine)
                )
                .Root(configurator => configurator
                    .RegisterFlag("verbose", "v", "Verbose description", false)
                    .RegisterFlag("clear", "Clear description", false)
                    .RegisterOption("option1", "o1", "Option 1 description", "default")
                    .RegisterOption("option2", "Option 2 description", "default2")
                    .RegisterParam("param1", "Parameter 1 description", ".")
                    .SetExecute((commandArgs, output) =>
                    {
                        output("Verbose: " + commandArgs.Flags["verbose"].ToString());
                        output("Clear: " + commandArgs.Flags["clear"].ToString());
                        output("Option1: " + commandArgs.Options["option1"]);
                        output("Option2: " + commandArgs.Options["option2"]);
                        output("Param1: " + commandArgs.Parameters["param1"]);
                    })
                    .RegisterSubcommand("show", "Show somethig", showC => showC
                        .RegisterParam("text", "Text to show", "Default text to show")
                        .SetExecute((commandArgs, outout) => outout(commandArgs.Parameters["text"]))
                        .RegisterSubcommand("subshow", "Show somethig", showCc => showCc
                            .RegisterParam("text2", "Text to show", "Default new text to show")
                            .SetExecute((commandArgs, outout) => outout(commandArgs.Parameters["text"]))
                        )
                    )
                    .RegisterSubcommand("list", "Show somethig", showC => showC
                        .RegisterParam("text", "Text to show", "Default text to show")
                        .SetExecute((commandArgs, outout) => outout(commandArgs.Parameters["text"]))
                    )
                )(args);
        }
    }
}