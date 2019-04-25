using System;
using System.Globalization;
using NFlags.Tests.DataTypes;
using NFlags.Tests.TestImplementations;
using Xunit;
using NFAssert = NFlags.Tests.Helpers.Assert;

namespace NFlags.Tests
{
    public class NFlagsRegisterCommandGenericTest
    {
        [Fact]
        public void RegisterCommandT_ShouldThrowIncorrectFlagMemberTypeException_IfMemberWithFlagAttributeIsNotBoolean()
        {
            Assert.Throws<IncorrectFlagMemberTypeException>(() =>
            {
                Cli
                    .Configure(c => { })
                    .Root<NotBooleanFlag>(c => { });
            });
        }

        [Fact]
        public void RegisterCommandT_ShouldThrowIncorrectParameterSeriesMemberTypeException_IfMemberWithParameterSeriesAttributeIsNotArray()
        {
            Assert.Throws<IncorrectParameterSeriesMemberTypeException>(() =>
            {
                Cli
                    .Configure(c => { })
                    .Root<NotArrayParameterSeries>(c => { });
            });
        }

        [Fact]
        public void RegisterCommandT_ShouldThrowPropertyWithoutSetterException_IfPropertyWithParameterSeriesAttributeHasNoSetter()
        {
            Assert.Throws<PropertyWithoutSetterException>(() =>
            {
                Cli
                    .Configure(c => { })
                    .Root<ParameterSeriesWithoutSetter>(c => { });
            });
        }

        [Fact]
        public void RegisterCommandT_ShouldThrowPropertyWithoutSetterException_IfPropertyWithFlagAttributeHasNoSetter()
        {
            Assert.Throws<PropertyWithoutSetterException>(() =>
            {
                Cli
                    .Configure(c => { })
                    .Root<FlagWithoutSetter>(c => { });
            });
        }

        [Fact]
        public void RegisterCommandT_ShouldThrowPropertyWithoutSetterException_IfPropertyWithOptionAttributeHasNoSetter()
        {
            Assert.Throws<PropertyWithoutSetterException>(() =>
            {
                Cli
                    .Configure(c => { })
                    .Root<OptionWithoutSetter>(c => { });
            });
        }

        [Fact]
        public void RegisterCommandT_ShouldThrowPropertyWithoutSetterException_IfPropertyWithParameterAttributeHasNoSetter()
        {
            Assert.Throws<PropertyWithoutSetterException>(() =>
            {
                Cli
                    .Configure(c => { })
                    .Root<ParameterWithoutSetter>(c => { });
            });
        }

        [Fact]
        public void RegisterCommandT_ShouldPrintParametersOptionsFlagsAndParameterSeriesInHelp()
        {
            var outputAggregator = new OutputAggregator();

            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetOutput(outputAggregator)
                )
                .Root<ArgumentsType>(c => c
                    .RegisterCommand<ArgumentsType>("sub", "sub command ", rc => rc
                        .SetExecute((args, output) => { }))
                    .SetExecute((type, output) => { })
                )
                .Run(new[] { "sub", "--help"});

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\ttesthost sub [OPTIONS]... [PARAMETERS]...",
                "",
                "\tParameters:",
                "\t<parameter1>\tparameter desc (Default: " + 1.1 + ")",
                "\t<parameter2>\tparameter2 desc (Default: " + 1 + ", Environment variable: 'NFLAG_TEST_PARAMETER2', Config path: 'Config:Path:Parameter2')",
                "\t<parameterSeries...>\tparameter series desc",
                "",
                "\tOptions:",
                "\t--option1 <option1>\toption desc (Default: " + 1 + ", Environment variable: 'NFLAG_TEST_OPTION1', Config path: 'Config:Path:Option1')",
                "\t--option2 <option2>, -o2 <option2>\toption2 desc (Default: 'asd')",
                "\t--flag1\tflag desc (Environment variable: 'NFLAG_TEST_FLAG1', Config path: 'Config:Path:Flag1')",
                "\t--flag2, -f2\tflag2 desc",
                "\t--help, -h\tPrints this help",
                ""
            );
        }

        [Fact]
        public void RegisterCommandT_ShouldPassDefaultValuesToExecute()
        {
            ArgumentsType a = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                )
                .Root<ArgumentsType>(c => c
                    .RegisterCommand<ArgumentsType>("sub", "sub command ", rc => rc
                        .SetExecute((args, output) =>
                        {
                            a = args;
                        }))
                    .SetExecute((type, output) => { })
                )
                .Run(new[] { "sub" });

            Assert.Equal(1, a.Option1);
            Assert.Equal("asd", a.Option2);
            Assert.True(a.Flag1);
            Assert.False(a.Flag2);
            Assert.Equal(1.1, a.Parameter1);
            Assert.Equal(1, a.Parameter2);
            Assert.Empty(a.ParameterSeries);
        }

        [Fact]
        public void RegisterCommandT_ShouldPassEnvironmentVariableToExecute_IfEnvironemntVariableIsSet()
        {
            var testEnvironment = new TestEnvironment()
                .SetEnvironmentVariable("NFLAG_TEST_OPTION1", "54")
                .SetEnvironmentVariable("NFLAG_TEST_FLAG1", "false")
                .SetEnvironmentVariable("NFLAG_TEST_PARAMETER2", "89");

            ArgumentsType a = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetEnvironment(testEnvironment)
                )
                .Root<ArgumentsType>(c => c
                    .RegisterCommand<ArgumentsType>("sub", "sub command ", rc => rc
                        .SetExecute((args, output) =>
                        {
                            a = args;
                        }))
                    .SetExecute((type, output) => { })
                )
                .Run(new[] { "sub" });

            Assert.Equal(54, a.Option1);
            Assert.Equal("asd", a.Option2);
            Assert.False(a.Flag1);
            Assert.False(a.Flag2);
            Assert.Equal(1.1, a.Parameter1);
            Assert.Equal(89, a.Parameter2);
            Assert.Empty(a.ParameterSeries);
        }

        [Fact]
        public void RegisterCommandT_ShouldPassValuesToExecute()
        {
            ArgumentsType a = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                )
                .Root<ArgumentsType>(c => c
                    .RegisterCommand<ArgumentsType>("sub", "sub command ", rc => rc
                        .SetExecute((args, output) =>
                        {
                            a = args;
                        }))
                    .SetExecute((type, output) => { })
                )
                .Run(new[]
                {
                    "sub",
                    "--flag1",
                    "-f2",
                    "--option1",
                    "3",
                    "-o2",
                    "xyz",
                    2.53.ToString(CultureInfo.CurrentCulture),
                    "5",
                    "6",
                    "7"
                });

            Assert.Equal(3, a.Option1);
            Assert.Equal("xyz", a.Option2);
            Assert.False(a.Flag1);
            Assert.True(a.Flag2);
            Assert.Equal(2.53, a.Parameter1);
            Assert.Equal(5, a.Parameter2);
            Assert.Equal(new[] {6, 7}, a.ParameterSeries);
        }

        [Fact]
        public void PrintHelp_ShouldPrintFlagAndOptionsInGroups()
        {
            var outputAggregator = new OutputAggregator();
            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                    .SetOutput(outputAggregator)
                    .SetName("groups")
                    .SetDescription("some description")
                )
                .Root<GroupedArgumentsType>(configurator => { })
                .Run(new[] { "--help" });

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
                "\t--flag2",
                "\t--flag4",
                "\t--flag7",
                "\t--option2 <option2>\t(Default: 0)",
                "\t--option4 <option4>\t(Default: 0)",
                "\t--option7 <option7>\t(Default: 0)",
                "\t--help, -h\tPrints this help",
                "",
                "\tgroup1:",
                "\t\t--flag1",
                "\t\t--flag5",
                "\t\t--option1 <option1>\t(Default: 0)",
                "\t\t--option3 <option3>\t(Default: 0)",
                "",
                "\tgroup2:",
                "\t\t--flag3",
                "\t\t--option5 <option5>\t(Default: 0)",
                "",
                "\tgroup3:",
                "\t\t--flag6",
                "\t\t--option6 <option6>\t(Default: 0)",
                ""
            );
        }
    }
}