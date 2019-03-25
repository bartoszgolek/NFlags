using NFlags.Tests.TestImplementations;
using Xunit;
using NFAssert = NFlags.Tests.Helpers.Assert;

namespace NFlags.Tests
{
    public abstract class DialectPrintHelp
    {
        private readonly Dialect _dialect;
        private readonly string _longPrefix;
        private readonly string _shortPrefix;
        private readonly string _optionValueSeparator;

        protected DialectPrintHelp(Dialect dialect, string shortPrefix, string longPrefix, string optionValueSeparator)
        {
            _dialect = dialect;
            _longPrefix = longPrefix;
            _shortPrefix = shortPrefix;
            _optionValueSeparator = optionValueSeparator;
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithDefaultName()
        {
            var outputAggregator = new OutputAggregator();
            NFlags.Configure(configurator =>
                {
                    configurator
                        .SetDialect(_dialect)
                        .SetOutput(outputAggregator);
                })
                .Root(c => { })
                .Run(new[] { "" + _longPrefix + "help" });

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\ttesthost [OPTIONS]...",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help",
                ""
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintSubCommandBasicInfoWithDefaultName()
        {
            var outputAggregator = new OutputAggregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAggregator)
                )
                .Root(c => { })
                .Run(new[] { "" + _longPrefix + "help" });

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\ttesthost [OPTIONS]...",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help",
                ""
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithCustomName()
        {
            var outputAggregator = new OutputAggregator();
            NFlags.Configure(configurator => configurator
                    .SetName("custName")
                    .SetDialect(_dialect)
                    .SetOutput(outputAggregator)
                )
                .Root(c => { })
                .Run(new[] { "" + _longPrefix + "help" });

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\tcustName [OPTIONS]...",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help",
                ""
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithDescription()
        {
            var outputAggregator = new OutputAggregator();
            NFlags.Configure(configurator => configurator
                    .SetDescription("some description")
                    .SetDialect(_dialect)
                    .SetOutput(outputAggregator)
                )
                .Root(c => { })
                .Run(new[] { "" + _longPrefix + "help" });

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\ttesthost [OPTIONS]...",
                "",
                "some description",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help",
                ""
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithFlags()
        {
            var outputAggregator = new OutputAggregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAggregator)
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "Flag 1 Description", false)
                    .RegisterFlag("flag2", "f2", "Flag 2 Description", false)
                )
                .Run(new[] { "" + _longPrefix + "help" });

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\ttesthost [OPTIONS]...",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "flag1\tFlag 1 Description",
                "\t" + _longPrefix + "flag2, " + _shortPrefix + "f2\tFlag 2 Description",
                "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help",
                ""
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithOptions()
        {
            var outputAggregator = new OutputAggregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAggregator)
                )
                .Root(configurator => configurator
                    .RegisterOption("option1", "Option 1 Description", "")
                    .RegisterOption("option2", "o2", "Option 2 Description", "")
                )
                .Run(new[] { "" + _longPrefix + "help" });

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\ttesthost [OPTIONS]...",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "option1" + _optionValueSeparator + "<option1>\tOption 1 Description (Default: '')",
                "\t" + _longPrefix + "option2" + _optionValueSeparator + "<option2>, " + _shortPrefix + "o2" + _optionValueSeparator + "<option2>\tOption 2 Description (Default: '')",
                "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help",
                ""
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithParams()
        {
            var outputAggregator = new OutputAggregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAggregator)
                )
                .Root(configurator => configurator
                    .RegisterParameter("param1", "Param 1 Description", "")
                    .RegisterParameter("param2", "Param 2 Description", "")
                )
                .Run(new[] { "" + _longPrefix + "help" });

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\ttesthost [OPTIONS]... [PARAMETERS]...",
                "",
                "\tParameters:",
                "\t<param1>\tParam 1 Description (Default: '')",
                "\t<param2>\tParam 2 Description (Default: '')",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help",
                ""
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithParametersAndParameterSeries()
        {
            var outputAggregator = new OutputAggregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAggregator)
                )
                .Root(configurator => configurator
                    .RegisterParameter("param1", "Param 1 Description", "")
                    .RegisterParameter("param2", "Param 2 Description", "")
                    .RegisterParameterSeries<string>("paramSeries", "Param series Description")
                )
                .Run(new[] { "" + _longPrefix + "help" });

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\ttesthost [OPTIONS]... [PARAMETERS]...",
                "",
                "\tParameters:",
                "\t<param1>\tParam 1 Description (Default: '')",
                "\t<param2>\tParam 2 Description (Default: '')",
                "\t<paramSeries...>\tParam series Description",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help",
                ""
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithParamSeries()
        {
            var outputAggregator = new OutputAggregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAggregator)
                )
                .Root(configurator => configurator
                    .RegisterParameterSeries<string>("paramSeries", "Param series Description")
                )
                .Run(new[] { "" + _longPrefix + "help" });

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\ttesthost [OPTIONS]... [PARAMETERS]...",
                "",
                "\tParameters:",
                "\t<paramSeries...>\tParam series Description",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help",
                ""
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithNameDescriptionFlagsOptionsAndParams()
        {
            var outputAggregator = new OutputAggregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAggregator)
                    .SetName("custName")
                    .SetDescription("some description")
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "Flag 1 Description", false)
                    .RegisterFlag("flag2", "f2", "Flag 2 Description", false)
                    .RegisterOption("option1", "Option 1 Description", "")
                    .RegisterOption("option2", "o2", "Option 2 Description", "")
                    .RegisterParameter("param1", "Param 1 Description", "")
                    .RegisterParameter("param2", "Param 2 Description", "")
                )
                .Run(new[] { "" + _longPrefix + "help" });

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\tcustName [OPTIONS]... [PARAMETERS]...",
                "",
                "some description",
                "",
                "\tParameters:",
                "\t<param1>\tParam 1 Description (Default: '')",
                "\t<param2>\tParam 2 Description (Default: '')",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "flag1\tFlag 1 Description",
                "\t" + _longPrefix + "flag2, " + _shortPrefix + "f2\tFlag 2 Description",
                "\t" + _longPrefix + "option1" + _optionValueSeparator + "<option1>\tOption 1 Description (Default: '')",
                "\t" + _longPrefix + "option2" + _optionValueSeparator + "<option2>, " + _shortPrefix + "o2" + _optionValueSeparator + "<option2>\tOption 2 Description (Default: '')",
                "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help",
                ""
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintDefaultValueAfterOptionAndParametersDescriptions()
        {
            var outputAggregator = new OutputAggregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAggregator)
                    .SetName("custName")
                    .SetDescription("some description")
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "Flag 1 Description", false)
                    .RegisterFlag("flag2", "f2", "Flag 2 Description", false)
                    .RegisterOption("option1", "Option 1 Description", 1)
                    .RegisterOption("option2", "o2", "Option 2 Description", 2.1)
                    .RegisterParameter("param1", "Param 1 Description", 8.5m)
                    .RegisterParameter("param2", "Param 2 Description", "default")
                    .RegisterParameter("param3", "", "default")
                )
                .Run(new[] { "" + _longPrefix + "help" });

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\tcustName [OPTIONS]... [PARAMETERS]...",
                "",
                "some description",
                "",
                "\tParameters:",
                "\t<param1>\tParam 1 Description (Default: "+ 8.5 + ")",
                "\t<param2>\tParam 2 Description (Default: 'default')",
                "\t<param3>\t(Default: 'default')",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "flag1\tFlag 1 Description",
                "\t" + _longPrefix + "flag2, " + _shortPrefix + "f2\tFlag 2 Description",
                "\t" + _longPrefix + "option1" + _optionValueSeparator + "<option1>\tOption 1 Description (Default: 1)",
                "\t" + _longPrefix + "option2" + _optionValueSeparator + "<option2>, " + _shortPrefix + "o2" + _optionValueSeparator + "<option2>\tOption 2 Description (Default: " + 2.1 + ")",
                "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help",
                ""
            );
        }
    }
}