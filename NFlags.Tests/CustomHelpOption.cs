using NFlags.Commands;
using NSubstitute;
using Xunit;

namespace NFlags.Tests
{
    public class CustomHelpOption
    {
        [Fact]
        public void Run_ShouldPrintHelp_WhenCustomizedHelpFlagPassed()
        {
            var printer = Substitute.For<IHelpPrinter>();

            Cli.Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .ConfigureHelp(configurator => configurator
                        .SetPrinter(printer)
                        .SetOptionFlag("xhelp")
                    )
                )
                .Root(c => c
                    .RegisterParameterSeries<string>("fk", "fake parameters to allow wrong flag without exception")
                    .SetExecute((args, output) => output.WriteLine(args.GetParameterSeries<string>()))
                )
                .Run(new[] { "--xhelp" });

            printer.Received().PrintHelp(Arg.Any<CommandConfig>());
        }

        [Fact]
        public void Run_ShouldPrintHelp_WhenCustomizedHelpFlagAbrPassed()
        {
            var printer = Substitute.For<IHelpPrinter>();

            Cli.Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .ConfigureHelp(configurator => configurator
                        .SetPrinter(printer)
                        .SetOptionAbr("x")
                    )
                )
                .Root(c => c
                    .RegisterParameterSeries<string>("fk", "fake parameters to allow wrong flag without exception")
                    .SetExecute((args, output) => output.WriteLine(args.GetParameterSeries<string>()))
                )
                .Run(new[] { "-x" });

            printer.Received().PrintHelp(Arg.Any<CommandConfig>());
        }

        [Fact]
        public void Run_ShouldNotPrintHelp_WhenCustomizedHelpFlagSetAndDefaultPassed()
        {
            var printer = Substitute.For<IHelpPrinter>();

            Cli.Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .ConfigureHelp(configurator => configurator
                        .SetPrinter(printer)
                        .SetOptionFlag("xhelp")
                    )
                )
                .Root(c => c
                    .RegisterParameterSeries<string>("fk", "fake parameters to allow wrong flag without exception")
                    .SetExecute((args, output) => output.WriteLine(args.GetParameterSeries<string>()))
                )
                .Run(new[] { "--help" });

            printer.DidNotReceive().PrintHelp(Arg.Any<CommandConfig>());
        }

        [Fact]
        public void Run_ShouldNotPrintHelp_WhenCustomizedHelpFlagAbrSetAndDefaultPassed()
        {
            var printer = Substitute.For<IHelpPrinter>();

            Cli.Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .ConfigureHelp(configurator => configurator
                        .SetPrinter(printer)
                        .SetOptionAbr("x")
                    )
                )
                .Root(c => c
                    .RegisterParameterSeries<string>("fk", "fake parameters to allow wrong flag without exception")
                    .SetExecute((args, output) => output.WriteLine(args.GetParameterSeries<string>()))
                )
                .Run(new[] { "-h" });

            printer.DidNotReceive().PrintHelp(Arg.Any<CommandConfig>());
        }
    }
}