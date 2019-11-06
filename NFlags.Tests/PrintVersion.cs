using System.Reflection;
using NFlags.Tests.TestImplementations;
using Xunit;
using NFAssert = NFlags.Tests.Helpers.Assert;

namespace NFlags.Tests
{
    public class PrintVersion
    {
        [Fact]
        public void PrintHelp_ShouldPrintVersion_WhenFlagPassed()
        {
            var outputAggregator = new OutputAggregator();
            Cli.Configure(configurator =>
                {
                    configurator
                        .SetDialect(Dialect.Gnu)
                        .ConfigureVersion(vc => vc.Enable())
                        .SetOutput(outputAggregator);
                })
                .Root(c => { })
                .Run(new[] { "--version" });

            NFAssert.HelpEquals(
                outputAggregator,
                "testhost Version: " + Assembly.GetEntryAssembly()?.GetName().Version
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintVersion_WhenFlagAbrPassed()
        {
            var outputAggregator = new OutputAggregator();
            Cli.Configure(configurator =>
                {
                    configurator
                        .SetDialect(Dialect.Gnu)
                        .ConfigureVersion(vc => vc.Enable())
                        .SetOutput(outputAggregator);
                })
                .Root(c => { })
                .Run(new[] { "-v" });

            NFAssert.HelpEquals(
                outputAggregator,
                "testhost Version: " + Assembly.GetEntryAssembly()?.GetName().Version
            );
        }
        [Fact]
        public void PrintHelp_ShouldPrintVersion_WhenCustomFlagPassed()
        {
            var outputAggregator = new OutputAggregator();
            Cli.Configure(configurator =>
                {
                    configurator
                        .SetDialect(Dialect.Gnu)
                        .ConfigureVersion(vc => vc.Enable().SetOptionFlag("xversion"))
                        .SetOutput(outputAggregator);
                })
                .Root(c => { })
                .Run(new[] { "--xversion" });

            NFAssert.HelpEquals(
                outputAggregator,
                "testhost Version: " + Assembly.GetEntryAssembly()?.GetName().Version
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintVersion_WhenCustomFlagAbrPassed()
        {
            var outputAggregator = new OutputAggregator();
            Cli.Configure(configurator =>
                {
                    configurator
                        .SetDialect(Dialect.Gnu)
                        .ConfigureVersion(vc => vc.Enable().SetOptionAbr("x"))
                        .SetOutput(outputAggregator);
                })
                .Root(c => { })
                .Run(new[] { "-x" });

            NFAssert.HelpEquals(
                outputAggregator,
                "testhost Version: " + Assembly.GetEntryAssembly()?.GetName().Version
            );
        }
    }
}