using NFlags.Commands;
using NSubstitute;
using Xunit;

namespace NFlags.Tests
{
    public class CustomHelpPrinterTest
    {
        [Fact]
        public void PrintingHelp_ShouldUseProvidedCustomHelpPrinter()
        {
            var printer = Substitute.For<IHelpPrinter>();

            NFlags.Configure(c => c
                    .SetHelpPrinter(printer)
                )
                .Root(c => c
                    .PrintHelpOnExecute()
                )
                .Run(new string[0]);

            printer.Received().PrintHelp(Arg.Any<CommandConfig>());
        }

        [Fact]
        public void PrintingHelp_ShouldPrintTextReturnedByPrinterToOutput()
        {
            const string helpText = "some help text";

            var printer = Substitute.For<IHelpPrinter>();
            printer.PrintHelp(Arg.Any<CommandConfig>()).Returns(helpText);

            var output = Substitute.For<IOutput>();
            
            NFlags.Configure(c => c
                    .SetHelpPrinter(printer)
                    .SetOutput(output)
                )
                .Root(c => c
                    .PrintHelpOnExecute()
                )
                .Run(new string[0]);

            output.Received().Write(Arg.Is(helpText));
        }
    }
}