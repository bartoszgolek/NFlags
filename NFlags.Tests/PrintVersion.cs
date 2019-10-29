using System.Reflection;
using NFlags.Tests.TestImplementations;
using Xunit;
using NFAssert = NFlags.Tests.Helpers.Assert;

namespace NFlags.Tests
{
    public class PrintVersion
    {
        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithDefaultName()
        {
            var outputAggregator = new OutputAggregator();
            Cli.Configure(configurator =>
                {
                    configurator
                        .SetDialect(Dialect.Gnu)
                        .EnableVersionOption()
                        .SetOutput(outputAggregator);
                })
                .Root(c => { })
                .Run(new[] { "--version" });

            NFAssert.HelpEquals(
                outputAggregator,
                "testhost Version: " + Assembly.GetEntryAssembly()?.GetName().Version
            );
        }
    }
}