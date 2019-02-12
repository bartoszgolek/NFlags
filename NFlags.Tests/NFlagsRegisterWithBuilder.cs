using System.Globalization;
using System.Linq;
using NFlags.Commands;
using NFlags.Tests.TestImplementations;
using Xunit;
using NFAssert = NFlags.Tests.Helpers.Assert;

namespace NFlags.Tests
{
    public class NFlagsRegisterWithBuilder
    {
        [Fact]
        public void RegisterCommand_ShouldPrintParametersOptionsFlagsAndParameterSeriesInHelp()
        {
            var outputAggregator = new OutputAggregator();

            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetOutput(outputAggregator)
                )
                .Root(c => c
                    .RegisterCommand("sub", "sub command ", rc => rc
                        .RegisterOption<int>(b => b.Name("option1").Description("option desc").DefaultValue(1))
                        .RegisterOption<string>(b => b.Name("option2").Abr("o2").Description("option2 desc").DefaultValue("asd"))
                        .RegisterFlag(b => b.Name("flag1").Description("flag desc").DefaultValue(true))
                        .RegisterFlag(b => b.Name("flag2").Abr("f2").Description("flag2 desc"))
                        .RegisterParameter<double>(b => b.Name("parameter1").Description("parameter desc").DefaultValue(1.1))
                        .RegisterParameter<int>(b => b.Name("parameter2").Description("parameter2 desc").DefaultValue(1))
                        .RegisterParameterSeries<int>(b => b.Name("parameterSeries").Description("parameter series desc"))
                        .SetExecute((args, output) => { }))
                    .SetExecute((type, output) => { })
                )
                .Run(new[] { "sub", "--help"});

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\ttesthost sub [FLAGS]... [OPTIONS]... [PARAMETERS]...",
                "",
                "\tFlags:",
                "\t--flag1\tflag desc",
                "\t--flag2, -f2\tflag2 desc",
                "\t--help, -h\tPrints this help",
                "",
                "\tOptions:",
                "\t--option1 <option1>\toption desc",
                "\t--option2 <option2>, -o2 <option2>\toption2 desc",
                "",
                "\tParameters:",
                "\t<parameter1>\tparameter desc",
                "\t<parameter2>\tparameter2 desc",
                "\t<parameterSeries...>\tparameter series desc",
                ""
            );
        }

        [Fact]
        public void RegisterCommandT_ShouldPassDefaultValuesToExecute()
        {
            CommandArgs a = null;
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                )
                .Root(c => c
                    .RegisterCommand("sub", "sub command ", rc => rc
                        .RegisterOption<int>(b => b.Name("option1").Description("option desc").DefaultValue(1))
                        .RegisterOption<string>(b => b.Name("option2").Abr("o2").Description("option2 desc").DefaultValue("asd"))
                        .RegisterFlag(b => b.Name("flag1").Description("flag desc").DefaultValue(true))
                        .RegisterFlag(b => b.Name("flag2").Abr("f2").Description("flag2 desc"))
                        .RegisterParameter<double>(b => b.Name("parameter1").Description("parameter desc").DefaultValue(1.1))
                        .RegisterParameter<int>(b => b.Name("parameter2").Description("parameter2 desc").DefaultValue(1))
                        .RegisterParameterSeries<int>(b => b.Name("parameterSeries").Description("parameter series desc"))
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
            Assert.Empty(a.GetParameterSeries<int>());
        }

        [Fact]
        public void RegisterCommandT_ShouldPassValuesToExecute()
        {
            CommandArgs a = null;
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                )
                .Root(c => c
                    .RegisterCommand("sub", "sub command ", rc => rc
                        .RegisterOption<int>(b => b.Name("option1").Description("option desc").DefaultValue(1))
                        .RegisterOption<string>(b => b.Name("option2").Abr("o2").Description("option2 desc").DefaultValue("asd"))
                        .RegisterFlag(b => b.Name("flag1").Description("flag desc").DefaultValue(true))
                        .RegisterFlag(b => b.Name("flag2").Abr("f2").Description("flag2 desc"))
                        .RegisterParameter<double>(b => b.Name("parameter1").Description("parameter desc").DefaultValue(1.1))
                        .RegisterParameter<int>(b => b.Name("parameter2").Description("parameter2 desc").DefaultValue(1))
                        .RegisterParameterSeries<int>(b => b.Name("parameterSeries").Description("parameter series desc"))
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

            Assert.Equal(3, a.GetOption<int>("option1"));
            Assert.Equal("xyz", a.GetOption<string>("option2"));
            Assert.False(a.GetFlag("flag1"));
            Assert.True(a.GetFlag("flag2"));
            Assert.Equal(2.53, a.GetParameter<double>("parameter1"));
            Assert.Equal(5, a.GetParameter<int>("parameter2"));
            Assert.Equal(new[] {6, 7}, a.GetParameterSeries<int>().ToArray());
        }
    }
}