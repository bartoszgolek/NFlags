using NFlags.Commands;
using Xunit;

namespace NFlags.Tests
{
    public class GnuDialectRead
    {
        [Fact]
        public void TestParams_ShouldSetOptionValue_IfContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "o", "", "")
                    .SetExecute((args, output) => Assert.Equal(args.Options["option"], "optValue"))
                )
                .Run(new[] {"--option", "optValue"});
        }

        [Fact]
        public void TestParams_ShouldSetDefaultOptionValue_IfNotContainedInArgs()
        {
            CommandArgs a = null;

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "o", "", "defaultValue")
                    .SetExecute((args, output) => a = args)
                )
                .Run(new string[] { });

            Assert.Equal(a.Options["option"], "defaultValue");
        }

        [Fact]
        public void TestParams_ShouldSetMultipleOptionValues_IfContainedInArgs()
        {
            CommandArgs a = null;  
            
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterOption("option1", "o1", "", "")
                    .RegisterOption("option2", "o2", "", "")
                    .SetExecute((args, output) => a = args)
                )
                .Run(new[] {"--option1", "optionValue1", "--option2", "optionValue2"});
            
            Assert.Equal(a.Options["option1"], "optionValue1");
            Assert.Equal(a.Options["option2"], "optionValue2");
        }

        [Fact]
        public void TestParams_ShouldSetOptionValue_IfAbrContainedInArgs()
        {
            CommandArgs a = null;  
            
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "o", "", "")
                    .SetExecute((args, output) => a = args)
                )
                .Run(new[] {"-o", "optionValue"});

            Assert.Equal(a.Options["option"], "optionValue");
        }

        [Fact]
        public void TestParams_ShouldSetMultipleOptionValues_IfAbrContainedInArgs()
        {
            CommandArgs a = null;  
            
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterOption("option1", "o1", "", "")
                    .RegisterOption("option2", "o2", "", "")
                    .SetExecute((args, output) => a = args)
                )
                .Run(new[] {"-o1", "optionValue1", "-o2", "optionValue2"});
            
            Assert.Equal(a.Options["option1"], "optionValue1");
            Assert.Equal(a.Options["option2"], "optionValue2");
        }

        [Fact]
        public void TestParams_ShouldSetMultipleOptionValues_IfOptionHasNoAbrAndContainedInArgs()
        {
            CommandArgs a = null;  
            
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterOption("option1", "", "")
                    .RegisterOption("option2", "", "")
                    .SetExecute((args, output) => a = args)
                )
                .Run(new[] {"--option1", "optValue1", "--option2", "optValue2"});

            Assert.Equal(a.Options["option1"], "optValue1");
            Assert.Equal(a.Options["option2"], "optValue2");
        }

        [Fact]
        public void TestParams_ShouldSetFlag_IfContainedInArgs()
        {
            CommandArgs a = null;  
            
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterFlag("xFlag", "x", "", false)
                    .SetExecute((args, output) => a = args)
                )
                .Run(new[] {"--xFlag"});

            Assert.True(a.Flags["xFlag"]);
        }

        [Fact]
        public void TestParams_ShouldNotSetFlagValue_IfNotContainedInArgs()
        {
            CommandArgs a = null;  
            
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterFlag("yFlag", "y", "", false)
                    .SetExecute((args, output) => a = args)
                )
                .Run(new string[] { });

            Assert.False(a.Flags["yFlag"]);
        }

        [Fact]
        public void TestParams_ShouldSetMultipleFlags_IfContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterFlag("xFlag1", "x1", "", false)
                    .RegisterFlag("xFlag2", "x2", "", false)
                )
                .Run(new[] {"--xFlag1", "--xFlag2"});
        }

        [Fact]
        public void TestParams_ShouldReverseFlags_IfContainedInArgs()
        {
            CommandArgs a = null;  
            
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterFlag("xFlag1", "x1", "", true)
                    .RegisterFlag("xFlag2", "x2", "", true)
                    .SetExecute((args, output) => a = args)
                )
                .Run(new[] {"--xFlag1", "--xFlag2"});
            
            Assert.False(a.Flags["xFlag1"]);
            Assert.False(a.Flags["xFlag2"]);
        }

        [Fact]
        public void TestParams_ShouldSetFlag_IfAbrContainedInArgs()
        {
            CommandArgs a = null;  
            
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterFlag("xFlag", "x", "", false)
                    .SetExecute((args, output) => a = args)
                )
                .Run(new[] {"-x"});

            Assert.True(a.Flags["xFlag"]);
        }

        [Fact]
        public void TestParams_ShouldSetMultipleFlags_IfAbrContainedInArgs()
        {
            CommandArgs a = null;  
            
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "f1", "", false)
                    .RegisterFlag("flag2", "f2", "", false)
                    .SetExecute((args, output) => a = args)
                )
                .Run(new[] {"-f1", "-f2"});

            Assert.True(a.Flags["flag1"]);
            Assert.True(a.Flags["flag2"]);
        }

        [Fact]
        public void TestParams_ShouldSetMultipleFlags_IfFlagHasNoAbrAndContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "", false)
                    .RegisterFlag("flag2", "", false)
                )
                .Run(new[] {"--flag1", "--flag2"});
        }

        [Fact]
        public void TestParams_ShouldSetFirstParam_IfContainedInArgs()
        {
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterParam("param", "", "")
                )
                .Run(new[] {"paramValue"});
        }

        [Fact]
        public void TestParams_ShouldSetSecondParam_IfContainedInArgs()
        {
            CommandArgs a = null;  
            
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterParam("param1", "", "")
                    .RegisterParam("param2", "", "")
                    .SetExecute((args, output) => a = args)
                )
                .Run(new[] {"paramValue1", "paramValue2"});
            
            Assert.Equal(a.Parameters["param1"], "paramValue1");
            Assert.Equal(a.Parameters["param2"], "paramValue2");
        }

        [Fact]
        public void TestParams_ShouldSetSecondFlagsOptionsAndParams()
        {
            CommandArgs a = null;  
            
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "f1", "", false)
                    .RegisterFlag("flag2", "", false)
                    .RegisterOption("option1", "", "")
                    .RegisterOption("option2", "o2", "", "")
                    .RegisterParam("param1", "", "")
                    .RegisterParam("param2", "", "")
                    .SetExecute((args, output) => a = args)
                )
                .Run(new[]
                {
                    "-f1",
                    "--flag2",
                    "--option1", "optionValue1",
                    "-o2", "optionValue2",
                    "paramValue1",
                    "paramValue2"
                });
            
            Assert.Equal(a.Flags["flag1"], true);
            Assert.Equal(a.Flags["flag2"], true);
            Assert.Equal(a.Options["option1"], "optionValue1");
            Assert.Equal(a.Options["option2"], "optionValue2");
            Assert.Equal(a.Parameters["param1"], "paramValue1");
            Assert.Equal(a.Parameters["param2"], "paramValue2");
        }
    }
}