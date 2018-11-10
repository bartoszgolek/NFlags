using NFlags.Tests.DataTypes;
using Xunit;
using NFAssert = NFlags.Tests.Helpers.Assert;

namespace NFlags.Tests
{
    public class NFlagsRootGenericTest
    {
        [Fact]
        public void RegisterCommandT_ShouldPrintParametersOptionsFlagsAndParameterSeriesInHelp()
        {
            var outputAggregator = new OutputAggregator();

            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetOutput(outputAggregator)
                )
                .Root<ArgumentsType>(c => { })
                .Run(new[] {"--help"});

            NFAssert.HelpEquals(
                outputAggregator,
                "Usage:",
                "\ttesthost [FLAGS]... [OPTIONS]... [PARAMETERS]...",
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
            ArgumentsType a = null;
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                )
                .Root<ArgumentsType>(c => c
                    .SetExecute((args, output) =>
                    {
                        a = args;
                    })
                )
                .Run(new string[] { });
            
            Assert.Equal(1, a.Option1);
            Assert.Equal("asd", a.Option2);
            Assert.True(a.Flag1);
            Assert.False(a.Flag2);
            Assert.Equal(1.1, a.Parameter1);
            Assert.Equal(1, a.Parameter2);
            Assert.Empty(a.ParameterSeries);
        }

        [Fact]
        public void RegisterCommandT_ShouldPassValuesToExecute()
        {
            ArgumentsType a = null;
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                )
                .Root<ArgumentsType>(c => c
                    .SetExecute((args, output) =>
                    {
                        a = args;
                    })
                )
                .Run(new string[]
                {
                    "--flag1",
                    "-f2",
                    "--option1",
                    "3",
                    "-o2",
                    "xyz",
                    "2,53",
                    "5",
                    "6",
                    "7",
                });
            
            Assert.Equal(3, a.Option1);
            Assert.Equal("xyz", a.Option2);
            Assert.False(a.Flag1);
            Assert.True(a.Flag2);
            Assert.Equal(2.53, a.Parameter1);
            Assert.Equal(5, a.Parameter2);
            Assert.Equal(new[] {6, 7}, a.ParameterSeries);
        }
    }
}