using NFlags.Commands;
using Xunit;

namespace NFlags.Tests
{
    public class WinDialectRead
    {
        [Fact]
        public void TestParams_ShouldSetOptionValue_IfContainedInArgs()
        {
            CommandArgs a = null;

            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "o", "", "")
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"/option=optValue"});

            Assert.Equal("optValue", a.GetOption<string>("option"));
        }

        [Fact]
        public void TestParams_ShouldSetDefaultOptionValue_IfNotContainedInArgs()
        {
            CommandArgs a = null;

            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "o", "", "defaultValue")
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new string[] { });

            Assert.Equal("defaultValue", a.GetOption<string>("option"));
        }

        [Fact]
        public void TestParams_ShouldSetMultipleOptionValues_IfContainedInArgs()
        {
            CommandArgs a = null;

            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option1", "o1", "", "")
                    .RegisterOption("option2", "o2", "", "")
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"/option1=optionValue1", "/option2=optionValue2"});

            Assert.Equal("optionValue1", a.GetOption<string>("option1"));
            Assert.Equal("optionValue2", a.GetOption<string>("option2"));
        }

        [Fact]
        public void TestParams_ShouldSetOptionValue_IfAbrContainedInArgs()
        {
            CommandArgs a = null;

            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "o", "", "")
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"/o=optionValue"});

            Assert.Equal("optionValue", a.GetOption<string>("option"));
        }

        [Fact]
        public void TestParams_ShouldSetOptionValue_IfWordAbrContainedInArgs()
        {
            CommandArgs a = null;

            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "op", "", "")
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"/op=optionValue"});

            Assert.Equal("optionValue", a.GetOption<string>("option"));
        }

        [Fact]
        public void TestParams_ShouldSetMultipleOptionValues_IfAbrContainedInArgs()
        {
            CommandArgs a = null;

            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option1", "o1", "", "")
                    .RegisterOption("option2", "o2", "", "")
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"/o1=optionValue1", "/o2=optionValue2"});

            Assert.Equal("optionValue1", a.GetOption<string>("option1"));
            Assert.Equal("optionValue2", a.GetOption<string>("option2"));
        }

        [Fact]
        public void TestParams_ShouldSetMultipleOptionValues_IfOptionHasNoAbrAndContainedInArgs()
        {
            CommandArgs a = null;

            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option1", "", "")
                    .RegisterOption("option2", "", "")
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"/option1=optValue1", "/option2=optValue2"});

            Assert.Equal("optValue1", a.GetOption<string>("option1"));
            Assert.Equal("optValue2", a.GetOption<string>("option2"));
        }

        [Fact]
        public void TestParams_ShouldSetFlag_IfContainedInArgs()
        {
            CommandArgs a = null;

            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterFlag("xFlag", "x", "", false)
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"/xFlag"});

            Assert.True(a.GetFlag("xFlag"));
        }

        [Fact]
        public void TestParams_ShouldNotSetFlagValue_IfNotContainedInArgs()
        {
            CommandArgs a = null;

            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterFlag("yFlag", "y", "", false)
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new string[] { });

            Assert.False(a.GetFlag("yFlag"));
        }

        [Fact]
        public void TestParams_ShouldSetMultipleFlags_IfContainedInArgs()
        {
            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterFlag("xFlag1", "x1", "", false)
                    .RegisterFlag("xFlag2", "x2", "", false)
                    .SetExecute((args, output) => { })
                )
                .Run(new[] {"/xFlag1", "/xFlag2"});
        }

        [Fact]
        public void TestParams_ShouldReverseFlags_IfContainedInArgs()
        {
            CommandArgs a = null;

            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterFlag("xFlag1", "x1", "", true)
                    .RegisterFlag("xFlag2", "x2", "", true)
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"/xFlag1", "/xFlag2"});

            Assert.False(a.GetFlag("xFlag1"));
            Assert.False(a.GetFlag("xFlag2"));
        }

        [Fact]
        public void TestParams_ShouldSetFlag_IfAbrContainedInArgs()
        {
            CommandArgs a = null;

            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterFlag("xFlag", "x", "", false)
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"/x"});

            Assert.True(a.GetFlag("xFlag"));
        }

        [Fact]
        public void TestParams_ShouldSetFlag_IfWordAbrContainedInArgs()
        {
            CommandArgs a = null;

            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterFlag("xFlag", "xf", "", false)
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"/xf"});

            Assert.True(a.GetFlag("xFlag"));
        }

        [Fact]
        public void TestParams_ShouldSetMultipleFlags_IfAbrContainedInArgs()
        {
            CommandArgs a = null;

            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "f1", "", false)
                    .RegisterFlag("flag2", "f2", "", false)
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"/f1", "/f2"});

            Assert.True(a.GetFlag("flag1"));
            Assert.True(a.GetFlag("flag2"));
        }

        [Fact]
        public void TestParams_ShouldSetMultipleFlags_IfFlagHasNoAbrAndContainedInArgs()
        {
            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "", false)
                    .RegisterFlag("flag2", "", false)
                    .SetExecute((args, output) => { })
                )
                .Run(new[] {"/flag1", "/flag2"});
        }

        [Fact]
        public void TestParams_ShouldSetFirstParam_IfContainedInArgs()
        {
            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterParameter("param", "", "")
                    .SetExecute((args, output) => { })
                )
                .Run(new[] {"paramValue"});
        }

        [Fact]
        public void TestParams_ShouldSetSecondParam_IfContainedInArgs()
        {
            CommandArgs a = null;

            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterParameter("param1", "", "")
                    .RegisterParameter("param2", "", "")
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"paramValue1", "paramValue2"});

            Assert.Equal("paramValue1", a.GetParameter<string>("param1"));
            Assert.Equal("paramValue2", a.GetParameter<string>("param2"));
        }

        [Fact]
        public void TestParams_ShouldSetParamSeries_IfContainedInArgs()
        {
            CommandArgs a = null;

            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterParameterSeries<string>("paramSeries1", "")
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"paramValue1"});

            Assert.Equal("paramValue1", a.GetParameterFromSeries<string>(0));
        }

        [Fact]
        public void TestParams_ShouldSetParamSeriesMultipleValues_IfContainedInArgs()
        {
            CommandArgs a = null;

            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterParameterSeries<string>("paramSeries1", "")
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"paramValue1", "paramValue2", "paramValue3"});

            Assert.Equal("paramValue1", a.GetParameterFromSeries<string>(0));
            Assert.Equal("paramValue2", a.GetParameterFromSeries<string>(1));
            Assert.Equal("paramValue3", a.GetParameterFromSeries<string>(2));
        }

        [Fact]
        public void TestParams_ShouldSetSecondFlagsOptionsAndParams()
        {
            CommandArgs a = null;

            Cli.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "f1", "", false)
                    .RegisterFlag("flag2", "", false)
                    .RegisterOption("option1", "", "")
                    .RegisterOption("option2", "o2", "", "")
                    .RegisterParameter("param1", "", "")
                    .RegisterParameter("param2", "", "")
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[]
                {
                    "/f1",
                    "/flag2",
                    "/option1=optionValue1",
                    "/o2=optionValue2",
                    "paramValue1",
                    "paramValue2"
                });

            Assert.True(a.GetFlag("flag1"));
            Assert.True(a.GetFlag("flag2"));
            Assert.Equal("optionValue1", a.GetOption<string>("option1"));
            Assert.Equal("optionValue2", a.GetOption<string>("option2"));
            Assert.Equal("paramValue1", a.GetParameter<string>("param1"));
            Assert.Equal("paramValue2", a.GetParameter<string>("param2"));
        }
    }
}