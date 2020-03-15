using NFlags.Tests.DataTypes;
using NFlags.Tests.TestImplementations;
using Xunit;
using NFAssert = NFlags.Tests.Helpers.Assert;

namespace NFlags.Tests
{
    public class PrintHelpOnExecute
    {
        [Fact]
        public void TestRun_ShouldPrintHelpWhenCommandExecuted()
        {
            var outputAggregator = new OutputAggregator();
            Cli
                .Configure(c => c.SetOutput(outputAggregator))
                .Root(c => c.PrintHelpOnExecute())
                .Run(new string[0]);

                NFAssert.HelpEquals(outputAggregator,
                    "Usage:",
                    "\ttesthost [OPTIONS]...",
                    "",
                    "\tOptions:",
                    "\t/help, /h\tPrints this help",
                    ""
                );
        }

        [Fact]
        public void TestRun_ShouldPrintHelpWhenGenericCommandExecuted()
        {
            var outputAggregator = new OutputAggregator();
            Cli
                .Configure(c => c.SetOutput(outputAggregator))
                .Root<EmptyArgumentsType>(c => c.PrintHelpOnExecute())
                .Run(new string[0]);

                NFAssert.HelpEquals(outputAggregator,
                    "Usage:",
                    "\ttesthost [OPTIONS]...",
                    "",
                    "\tOptions:",
                    "\t/help, /h\tPrints this help",
                    ""
                );
        }

        [Fact]
        public void TestRun_ShouldPrintRootCommandHelp_IfDefaultCommandIsSetAndHelpFlagIsPassed()
        {
            var outputAggregator = new OutputAggregator();
            Cli
                .Configure(c => c
                    .SetOutput(outputAggregator)
                    .SetDialect(Dialect.Gnu)
                )
                .Root(c => c
                    .RegisterDefaultCommand(
                        "command",
                        "description",
                        configurator => configurator
                            .PrintHelpOnExecute()
                    )
                )
                .Run(new [] { "-h" });

            NFAssert.HelpEquals(outputAggregator,
                "Usage:",
                "\ttesthost [COMMAND] [OPTIONS]...",
                "",
                "\tCommands:",
                "\tcommand\tdescription",
                "",
                "\tOptions:",
                "\t--help, -h\tPrints this help",
                ""
            );
        }

        [Fact]
        public void TestRun_ShouldPrintGenericRootCommandHelp_IfDefaultCommandIsSetAndHelpFlagIsPassed()
        {
            var outputAggregator = new OutputAggregator();
            Cli
                .Configure(c => c
                    .SetOutput(outputAggregator)
                    .SetDialect(Dialect.Gnu)
                )
                .Root<EmptyArgumentsType>(c => c
                    .RegisterDefaultCommand<EmptyArgumentsType>(
                        "command",
                        "description",
                        configurator => configurator
                            .PrintHelpOnExecute()
                    )
                )
                .Run(new [] { "-h" });

            NFAssert.HelpEquals(outputAggregator,
                "Usage:",
                "\ttesthost [COMMAND] [OPTIONS]...",
                "",
                "\tCommands:",
                "\tcommand\tdescription",
                "",
                "\tOptions:",
                "\t--help, -h\tPrints this help",
                ""
            );
        }

        [Fact]
        public void TestRun_ShouldPrintSubCommandHelp_IfPrintHelpOnExecuteIsSetForSubCommand()
        {
            var outputAggregator = new OutputAggregator();
            Cli
                .Configure(c => c.SetOutput(outputAggregator))
                .Root<EmptyArgumentsType>(c => c
                    .PrintHelpOnExecute()
                    .RegisterCommand<EmptyArgumentsType>("command", "description", configurator => configurator
                        .PrintHelpOnExecute()
                    )
                )
                .Run(new [] { "command" });

                NFAssert.HelpEquals(outputAggregator,
                    "Usage:",
                    "\ttesthost command [OPTIONS]...",
                    "",
                    "\tOptions:",
                    "\t/help, /h\tPrints this help",
                    ""
                );
        }

        [Fact]
        public void TestRun_ShouldThrowMissingCommandImplementationException_IfBothPrintHelpOnExecuteAndSetExecuteAreNotSet()
        {
            Assert.Throws<MissingCommandImplementationException>(() =>
            {
                Cli
                    .Configure(c => { })
                    .Root(c => { })
                    .Run(new string[0]);
            });
        }
    }
}