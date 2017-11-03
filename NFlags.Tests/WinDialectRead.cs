using Xunit;

namespace NFlags.Tests
{
    public class WinDialectRead
    {
        [Fact]
        public void TestParams_ShouldSetOptionValue_IfContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "o", "", "")
                    .SetExecute((args, output) => Assert.Equal(args.Options["option"], "optValue"))
                )(new[] {"/option=optValue"});
        }

        [Fact]
        public void TestParams_ShouldSetDefaultOptionValue_IfNotContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "o", "", "defaultValue")
                    .SetExecute((args, output) => Assert.Equal(args.Options["option"], "defaultValue"))
                )(new string[] { });
        }

        [Fact]
        public void TestParams_ShouldSetMultipleOptionValues_IfContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option1", "o1", "", "")
                    .RegisterOption("option2", "o2", "", "")
                    .SetExecute((args, output) =>
                    {                        
                        Assert.Equal(args.Options["option1"], "optionValue1");
                        Assert.Equal(args.Options["option2"], "optionValue2");
                    })
                )(new[] {"/option1=optValue1", "/option2=optValue2"});
        }

        [Fact]
        public void TestParams_ShouldSetOptionValue_IfAbrContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "o", "", "")
                    .SetExecute((args, output) => Assert.Equal(args.Options["option1"], "optionValue1"))
                )(new[] {"/o=optValue"});
        }

        [Fact]
        public void TestParams_ShouldSetMultipleOptionValues_IfAbrContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option1", "o1", "", "")
                    .RegisterOption("option2", "o2", "", "")
                    .SetExecute((args, output) =>
                    {                        
                        Assert.Equal(args.Options["option1"], "optionValue1");
                        Assert.Equal(args.Options["option2"], "optionValue2");
                    })
                )(new[] {"/o1=optValue1", "/o2=optValue2"});
        }

        [Fact]
        public void TestParams_ShouldSetMultipleOptionValues_IfOptionHasNoAbrAndContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option1", "", "")
                    .RegisterOption("option2", "", "")
                    .SetExecute((args, output) =>
                    {                        
                        Assert.Equal(args.Options["option1"], "optionValue1");
                        Assert.Equal(args.Parameters["option2"], "optionValue2");
                    })
                )(new[] {"/option1=optValue1", "/option2=optValue2"});
        }

        [Fact]
        public void TestParams_ShouldSetFlag_IfContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterFlag("xFlag", "x", "", false)
                    .SetExecute((args, output) => Assert.True(args.Flags["xFlag"]))
                )(new[] {"/xFlag"});
        }

        [Fact]
        public void TestParams_ShouldNotSetFlagValue_IfNotContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterFlag("yFlag", "y", "", false)
                    .SetExecute((args, output) => Assert.False(args.Flags["xFlag"]))
                )(new string[] { });
        }

        [Fact]
        public void TestParams_ShouldSetMultipleFlags_IfContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterFlag("xFlag1", "x1", "", false)
                    .RegisterFlag("xFlag2", "x2", "", false)
                )(new[] {"/xFlag1", "/xFlag2"});
        }

        [Fact]
        public void TestParams_ShouldReverseFlags_IfContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterFlag("xFlag1", "x1", "", true)
                    .RegisterFlag("xFlag2", "x2", "", true)
                    .SetExecute((args, output) =>
                    {                        
                        Assert.False(args.Flags["xFlag"]);
                        Assert.False(args.Flags["xFlag"]);
                    })
                )(new[] {"/xFlag1", "/xFlag2"});
        }

        [Fact]
        public void TestParams_ShouldSetFlag_IfAbrContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterFlag("xFlag", "x", "", false)
                    .SetExecute((args, output) =>
                    {                        
                        Assert.True(args.Flags["xFlag"]);
                    })
                )(new[] {"/x"});
        }

        [Fact]
        public void TestParams_ShouldSetMultipleFlags_IfAbrContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "f1", "", false)
                    .RegisterFlag("flag2", "f2", "", false)
                    .SetExecute((args, output) =>
                    {                        
                        Assert.True(args.Flags["flag1"]);
                        Assert.True(args.Flags["flag2"]);
                    })
                )(new[] {"/f1", "/f2"});
        }

        [Fact]
        public void TestParams_ShouldSetMultipleFlags_IfFlagHasNoAbrAndContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "", false)
                    .RegisterFlag("flag2", "", false)
                )(new[] {"/flag1", "/flag2"});
        }

        [Fact]
        public void TestParams_ShouldSetFirstParam_IfContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterParam("param", "", "")
                )(new[] {"paramValue"});
        }

        [Fact]
        public void TestParams_ShouldSetSecondParam_IfContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterParam("param1", "", "")
                    .RegisterParam("param2", "", "")
                    .SetExecute((args, output) =>
                    {                        
                        Assert.Equal(args.Parameters["param1"], "paramValue1");
                        Assert.Equal(args.Parameters["param2"], "paramValue2");
                    })
                )(new[] {"paramValue1", "paramValue2"});
        }

        [Fact]
        public void TestParams_ShouldSetSecondFlagsOptionsAndParams()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "f1", "", false)
                    .RegisterFlag("flag2", "", false)
                    .RegisterOption("option1", "", "")
                    .RegisterOption("option2", "o2", "", "")
                    .RegisterParam("param1", "", "")
                    .RegisterParam("param2", "", "")
                    .SetExecute((args, output) =>
                    {                        
                        Assert.Equal(args.Flags["flag1"], true);
                        Assert.Equal(args.Flags["flag2"], true);
                        Assert.Equal(args.Options["option1"], "optionValue1");
                        Assert.Equal(args.Options["option2"], "optionValue2");
                        Assert.Equal(args.Parameters["param1"], "paramValue1");
                        Assert.Equal(args.Parameters["param2"], "paramValue2");
                    })
                )(new[]
                {
                    "/f1",
                    "/flag2",
                    "/option1=optionValue1",
                    "/o2=optionValue2",
                    "paramValue1",
                    "paramValue2"
                });
        }
    }
}