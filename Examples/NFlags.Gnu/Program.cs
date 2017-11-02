using System;

namespace NFlags.Gnu
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var rootCommand = NFlags.Configure(configurator => configurator
                    .SetDescription("Application description")
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterFlag("help", "h", "Print this help", false)
                    .RegisterFlag("verbose", "v", "Verbose description", false)
                    .RegisterFlag("clear", "Clear description", false)
                    .RegisterOption("option1", "o1", "Option 1 description", "default")
                    .RegisterOption("option2", "Option 2 description", "default2")
                    .RegisterParam("param1", "Parameter 1 description", ".")
                );

            try
            {
                rootCommand.Read(args);

//                if (printHelp)
//                {
//                    Console.WriteLine(rootCommand.PrintHelp());
//                    return;
//                }

//                Console.WriteLine("Option1: " + option1);
//                Console.WriteLine("Option2: " + option2);
//                Console.WriteLine("C: " + flagC);
//                Console.WriteLine("V: " + flagV);
//                Console.WriteLine("Param1: " + param1);
            }
            catch (TooManyParametersException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(rootCommand.PrintHelp());
            }
        }
    }
}