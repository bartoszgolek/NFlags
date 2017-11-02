using System;
using NFlags.Dialects;

namespace NFlags.Gnu
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var flagV = false;
            var flagC = false;
            var option1 = "default";
            var option2 = "default2";
            var param1 = ".";
            var printHelp = false;

            var rootCommand = NFlags.Configure(configurator => configurator
                    .SetDescription("Application description")
                    .SetDialect(new GnuDialect())
                )
                .Root(configurator => configurator
                    .RegisterFlag("help", "h", "Print this help", () => printHelp = true)
                    .RegisterFlag("verbose", "v", "Verbose description", () => flagV = true)
                    .RegisterFlag("clear", "Clear description", () => flagC = true)
                    .RegisterOption("option1", "o1", "Option 1 description", option => option1 = option)
                    .RegisterOption("option2", "Option 2 description", option => option2 = option)
                    .RegisterParam("param1", "Parameter 1 description", param => param1 = param)
                );

            try
            {
                rootCommand.Read(args);

                if (printHelp)
                {
                    Console.WriteLine(rootCommand.PrintHelp());
                    return;
                }

                Console.WriteLine("Option1: " + option1);
                Console.WriteLine("Option2: " + option2);
                Console.WriteLine("C: " + flagC);
                Console.WriteLine("V: " + flagV);
                Console.WriteLine("Param1: " + param1);
            }
            catch (TooManyParametersException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(rootCommand.PrintHelp());
            }
        }
    }
}