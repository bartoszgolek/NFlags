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
            Cli.Configure(configurator =>
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
        public void PrintHelp_ShouldPrintInfoOfVersionOption_WhenEnabled()
        {
            var outputAggregator = new OutputAggregator();
            Cli.Configure(configurator =>
                {
                    configurator
                        .SetDialect(_dialect)
                        .SetOutput(outputAggregator)
                        .EnableVersionOption();
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
                "\t" + _longPrefix + "version, " + _shortPrefix + "v\tPrints application version",
                ""
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintSubCommandBasicInfoWithDefaultName()
        {
            var outputAggregator = new OutputAggregator();
            Cli.Configure(configurator => configurator
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
            Cli.Configure(configurator => configurator
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
            Cli.Configure(configurator => configurator
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
            Cli.Configure(configurator => configurator
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
            Cli.Configure(configurator => configurator
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
            Cli.Configure(configurator => configurator
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
            Cli.Configure(configurator => configurator
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
            Cli.Configure(configurator => configurator
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
            Cli.Configure(configurator => configurator
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
            Cli.Configure(configurator => configurator
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

        [Fact]
        public void PrintHelp_ShouldPrintFlagAndOptionsInGroups()
        {
            var outputAggregator = new OutputAggregator();
            Cli.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAggregator)
                    .SetName("groups")
                    .SetDescription("some description")
                )
                .Root(configurator => configurator
                    .RegisterFlag(b => b.Name("flag1").Group("group1"))
                    .RegisterFlag(b => b.Name("flag2"))
                    .RegisterFlag(b => b.Name("flag3").Group("group2"))
                    .RegisterFlag(b => b.Name("flag4"))
                    .RegisterFlag(b => b.Name("flag5").Group("group1"))
                    .RegisterFlag(b => b.Name("flag6").Group("group3"))
                    .RegisterFlag(b => b.Name("flag7"))
                    .RegisterOption<int>(b => b.Name("option1").Group("group1"))
                    .RegisterOption<int>(b => b.Name("option2"))
                    .RegisterOption<int>(b => b.Name("option3").Group("group1"))
                    .RegisterOption<int>(b => b.Name("option4"))
                    .RegisterOption<int>(b => b.Name("option5").Group("group2"))
                    .RegisterOption<int>(b => b.Name("option6").Group("group3"))
                    .RegisterOption<int>(b => b.Name("option7"))
                    .RegisterParameter<string>(b => b.Name("param1"))
                    .RegisterParameter<string>(b => b.Name("param2"))
                    .RegisterParameter<string>(b => b.Name("param3"))
                )
                .Run(new[] { "" + _longPrefix + "help" });

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\tgroups [OPTIONS]... [PARAMETERS]...",
                "",
                "some description",
                "",
                "\tParameters:",
                "\t<param1>",
                "\t<param2>",
                "\t<param3>",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "flag2",
                "\t" + _longPrefix + "flag4",
                "\t" + _longPrefix + "flag7",
                "\t" + _longPrefix + "option2" + _optionValueSeparator + "<option2>\t(Default: 0)",
                "\t" + _longPrefix + "option4" + _optionValueSeparator + "<option4>\t(Default: 0)",
                "\t" + _longPrefix + "option7" + _optionValueSeparator + "<option7>\t(Default: 0)",
                "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help",
                "",
                "\tgroup1:",
                "\t\t" + _longPrefix + "flag1",
                "\t\t" + _longPrefix + "flag5",
                "\t\t" + _longPrefix + "option1" + _optionValueSeparator + "<option1>\t(Default: 0)",
                "\t\t" + _longPrefix + "option3" + _optionValueSeparator + "<option3>\t(Default: 0)",
                "",
                "\tgroup2:",
                "\t\t" + _longPrefix + "flag3",
                "\t\t" + _longPrefix + "option5" + _optionValueSeparator + "<option5>\t(Default: 0)",
                "",
                "\tgroup3:",
                "\t\t" + _longPrefix + "flag6",
                "\t\t" + _longPrefix + "option6" + _optionValueSeparator + "<option6>\t(Default: 0)",
                ""
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintCustomHelpFlag()
        {
            var outputAggregator = new OutputAggregator();
            Cli.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAggregator)
                    .SetName("groups")
                    .SetDescription("some description")
                    .ConfigureHelp(hc => hc.SetOptionFlag("xhelp"))
                )
                .Root(configurator => configurator.PrintHelpOnExecute())
                .Run(new string[0]);

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\tgroups [OPTIONS]...",
                "",
                "some description",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "xhelp, " + _shortPrefix + "h\tPrints this help",
                ""
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintCustomHelpFlagAbbreviation()
        {
            var outputAggregator = new OutputAggregator();
            Cli.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAggregator)
                    .SetName("groups")
                    .SetDescription("some description")
                    .ConfigureHelp(hc => hc.SetOptionAbr("x"))
                )
                .Root(configurator => configurator.PrintHelpOnExecute())
                .Run(new string[0]);

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\tgroups [OPTIONS]...",
                "",
                "some description",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "help, " + _shortPrefix + "x\tPrints this help",
                ""
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintCustomHelpDescription()
        {
            var outputAggregator = new OutputAggregator();
            Cli.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAggregator)
                    .SetName("groups")
                    .SetDescription("some description")
                    .ConfigureHelp(hc => hc.SetOptionDescription("custom help description"))
                )
                .Root(configurator => configurator.PrintHelpOnExecute())
                .Run(new string[0]);

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\tgroups [OPTIONS]...",
                "",
                "some description",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "help, " + _shortPrefix + "h\tcustom help description",
                ""
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintCustomVersionFlag()
        {
            var outputAggregator = new OutputAggregator();
            Cli.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAggregator)
                    .SetName("groups")
                    .SetDescription("some description")
                    .ConfigureVersion(vc => vc.Enable().SetOptionFlag("xversion"))
                )
                .Root(configurator => configurator.PrintHelpOnExecute())
                .Run(new string[0]);

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\tgroups [OPTIONS]...",
                "",
                "some description",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help",
                "\t" + _longPrefix + "xversion, " + _shortPrefix + "v\tPrints application version",
                ""
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintCustomVersionFlagAbbreviation()
        {
            var outputAggregator = new OutputAggregator();
            Cli.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAggregator)
                    .SetName("groups")
                    .SetDescription("some description")
                    .ConfigureVersion(vc => vc.Enable().SetOptionAbr("x"))
                )
                .Root(configurator => configurator.PrintHelpOnExecute())
                .Run(new string[0]);

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\tgroups [OPTIONS]...",
                "",
                "some description",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help",
                "\t" + _longPrefix + "version, " + _shortPrefix + "x\tPrints application version",
                ""
            );
        }

        [Fact]
        public void PrintHelp_ShouldPrintCustomVersionDescription()
        {
            var outputAggregator = new OutputAggregator();
            Cli.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAggregator)
                    .SetName("groups")
                    .SetDescription("some description")
                    .ConfigureVersion(vc => vc.Enable().SetOptionDescription("custom version description"))
                )
                .Root(configurator => configurator.PrintHelpOnExecute())
                .Run(new string[0]);

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\tgroups [OPTIONS]...",
                "",
                "some description",
                "",
                "\tOptions:",
                "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help",
                "\t" + _longPrefix + "version, " + _shortPrefix + "v\tcustom version description",
                ""
            );
        }
    }
}