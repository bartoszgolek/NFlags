using System.Globalization;
using NFlags.Commands;
using NFlags.Tests.TestImplementations;
using Xunit;
using NFAssert = NFlags.Tests.Helpers.Assert;

namespace NFlags.Tests
{
    public class NFlagsRegisterWithConfigValues
    {
        [Fact]
        public void RegisterCommand_ShouldPassDefaultValuesToExecute()
        {
            var testConfig = new TestConfig();

            CommandArgs a = null;
            Cli
                .Configure(c =>
                {
                    c
                        .SetDialect(Dialect.Gnu)
                        .SetConfiguration(testConfig);
                })
                .Root(c => c
                    .RegisterCommand("sub", "sub command ", rc => rc
                        .RegisterOption<int>(b => b.Name("option1").Description("option desc").DefaultValue(1).ConfigPath("NFLAG_TEST_OPTION1"))
                        .RegisterOption<string>(b => b.Name("option2").Abr("o2").Description("option2 desc").DefaultValue("asd").ConfigPath("NFLAG_TEST_OPTION2"))
                        .RegisterFlag(b => b.Name("flag1").Description("flag desc").DefaultValue(true).ConfigPath("NFLAG_TEST_FLAG1"))
                        .RegisterFlag(b => b.Name("flag2").Abr("f2").Description("flag2 desc").ConfigPath("NFLAG_TEST_FLAG2"))
                        .RegisterParameter<double>(b => b.Name("parameter1").Description("parameter desc").DefaultValue(1.1).ConfigPath("NFLAG_TEST_PARAMETER1"))
                        .RegisterParameter<int>(b => b.Name("parameter2").Description("parameter2 desc").DefaultValue(1).ConfigPath("NFLAG_TEST_PARAMETER2"))
                        .SetExecute((args, output) =>
                        {
                            a = args;
                        }))
                    .SetExecute((type, output) => { })
                )
                .Run(new[] { "sub" });

            Assert.Equal(1, a.GetOption<int>("option1"));
            Assert.Equal("asd", a.GetOption<string>("option2"));
            Assert.True(a.GetFlag("flag1"));
            Assert.False(a.GetFlag("flag2"));
            Assert.Equal(1.1, a.GetParameter<double>("parameter1"));
            Assert.Equal(1, a.GetParameter<int>("parameter2"));
        }

        [Fact]
        public void RegisterCommand_ShouldPassConfigurationValuesToValuesToExecute()
        {
            var testConfig = new TestConfig()
                .SetConfigValue("NFLAG_TEST_FLAG1", "false")
                .SetConfigValue("NFLAG_TEST_FLAG2", "true")
                .SetConfigValue("NFLAG_TEST_OPTION1", "3")
                .SetConfigValue("NFLAG_TEST_OPTION2", "xyz")
                .SetConfigValue("NFLAG_TEST_PARAMETER1", 2.53.ToString(CultureInfo.CurrentCulture))
                .SetConfigValue("NFLAG_TEST_PARAMETER2", "5");

            CommandArgs a = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetConfiguration(testConfig)
                )
                .Root(c => c
                    .RegisterCommand("sub", "sub command ", rc => rc
                        .RegisterOption<int>(b => b.Name("option1").Description("option desc").DefaultValue(1).ConfigPath("NFLAG_TEST_OPTION1"))
                        .RegisterOption<string>(b => b.Name("option2").Abr("o2").Description("option2 desc").DefaultValue("asd").ConfigPath("NFLAG_TEST_OPTION2"))
                        .RegisterFlag(b => b.Name("flag1").Description("flag desc").DefaultValue(true).ConfigPath("NFLAG_TEST_FLAG1"))
                        .RegisterFlag(b => b.Name("flag2").Abr("f2").Description("flag2 desc").ConfigPath("NFLAG_TEST_FLAG2"))
                        .RegisterParameter<double>(b => b.Name("parameter1").Description("parameter desc").DefaultValue(1.1).ConfigPath("NFLAG_TEST_PARAMETER1"))
                        .RegisterParameter<int>(b => b.Name("parameter2").Description("parameter2 desc").DefaultValue(1).ConfigPath("NFLAG_TEST_PARAMETER2"))
                        .SetExecute((args, output) =>
                        {
                            a = args;
                        }))
                    .SetExecute((type, output) => { })
                )
                .Run(new[] { "sub" });

            Assert.Equal(3, a.GetOption<int>("option1"));
            Assert.Equal("xyz", a.GetOption<string>("option2"));
            Assert.False(a.GetFlag("flag1"));
            Assert.True(a.GetFlag("flag2"));
            Assert.Equal(2.53, a.GetParameter<double>("parameter1"));
            Assert.Equal(5, a.GetParameter<int>("parameter2"));
        }

        [Fact]
        public void RegisterCommand_ShouldPassArgumentsToValuesToExecute_EvenIfConfigurationValuesAreSet()
        {
            var testEnvironment = new TestConfig()
                .SetConfigValue("NFLAG_TEST_OPTION1", "3")
                .SetConfigValue("NFLAG_TEST_OPTION2", "xyz")
                .SetConfigValue("NFLAG_TEST_PARAMETER1", 2.53.ToString(CultureInfo.CurrentCulture))
                .SetConfigValue("NFLAG_TEST_PARAMETER2", "5");

            CommandArgs a = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetConfiguration(testEnvironment)
                )
                .Root(c => c
                    .RegisterCommand("sub", "sub command ", rc => rc
                        .RegisterOption<int>(b => b.Name("option1").Description("option desc").DefaultValue(1).ConfigPath("NFLAG_TEST_OPTION1"))
                        .RegisterOption<string>(b => b.Name("option2").Abr("o2").Description("option2 desc").DefaultValue("asd").ConfigPath("NFLAG_TEST_OPTION2"))
                        .RegisterParameter<double>(b => b.Name("parameter1").Description("parameter desc").DefaultValue(1.1).ConfigPath("NFLAG_TEST_PARAMETER1"))
                        .RegisterParameter<int>(b => b.Name("parameter2").Description("parameter2 desc").DefaultValue(1).ConfigPath("NFLAG_TEST_PARAMETER2"))
                        .SetExecute((args, output) =>
                        {
                            a = args;
                        }))
                    .SetExecute((type, output) => { })
                )
                .Run(new[]
                {
                    "sub",
                    "--option1",
                    "30",
                    "-o2",
                    "cxyz",
                    20.53.ToString(CultureInfo.CurrentCulture),
                    "50"
                });

            Assert.Equal(30, a.GetOption<int>("option1"));
            Assert.Equal("cxyz", a.GetOption<string>("option2"));
            Assert.Equal(20.53, a.GetParameter<double>("parameter1"));
            Assert.Equal(50, a.GetParameter<int>("parameter2"));
        }
    }
}