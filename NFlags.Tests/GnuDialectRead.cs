using Moq;
using Xunit;

namespace NFlags.Tests
{
    public class GnuDialectRead
    {
        [Fact]
        public void TestParams_ShouldSetOptionValue_IfContainedInArgs()
        {
            var mock = new Mock<IArgMock>();

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "o", "", mock.Object.Option)
                )
                .Read(new[] {"--option", "optValue"});

            mock.Verify(m => m.Option("optValue"));
        }

        [Fact]
        public void TestParams_ShouldNotSetOptionValue_IfNotContainedInArgs()
        {
            var mock = new Mock<IArgMock>(MockBehavior.Strict);

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "o", "", mock.Object.Option)
                )
                .Read(new string[] { });
        }

        [Fact]
        public void TestParams_ShouldSetMultipleOptionValues_IfContainedInArgs()
        {
            var mock1 = new Mock<IArgMock>();
            var mock2 = new Mock<IArgMock>();

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterOption("option1", "o1", "", mock1.Object.Option)
                    .RegisterOption("option2", "o2", "", mock2.Object.Option)
                )
                .Read(new[] {"--option1", "optValue1", "--option2", "optValue2"});

            mock1.Verify(m => m.Option("optValue1"));
            mock2.Verify(m => m.Option("optValue2"));
        }

        [Fact]
        public void TestParams_ShouldSetOptionValue_IfAbrContainedInArgs()
        {
            var mock = new Mock<IArgMock>();

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "o", "", mock.Object.Option)
                )
                .Read(new[] {"-o", "optValue"});

            mock.Verify(m => m.Option("optValue"));
        }

        [Fact]
        public void TestParams_ShouldSetMultipleOptionValues_IfAbrContainedInArgs()
        {
            var mock1 = new Mock<IArgMock>();
            var mock2 = new Mock<IArgMock>();

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterOption("option1", "o1", "", mock1.Object.Option)
                    .RegisterOption("option2", "o2", "", mock2.Object.Option)
                )
                .Read(new[] {"-o1", "optValue1", "-o2", "optValue2"});

            mock1.Verify(m => m.Option("optValue1"));
            mock2.Verify(m => m.Option("optValue2"));
        }

        [Fact]
        public void TestParams_ShouldSetMultipleOptionValues_IfOptionHasNoAbrAndContainedInArgs()
        {
            var mock1 = new Mock<IArgMock>();
            var mock2 = new Mock<IArgMock>();

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterOption("option1", "", mock1.Object.Option)
                    .RegisterOption("option2", "", mock2.Object.Option)
                )
                .Read(new[] {"--option1", "optValue1", "--option2", "optValue2"});

            mock1.Verify(m => m.Option("optValue1"));
            mock2.Verify(m => m.Option("optValue2"));
        }

        [Fact]
        public void TestParams_ShouldSetFlag_IfContainedInArgs()
        {
            var mock = new Mock<IArgMock>();

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterFlag("xFlag", "x", "", mock.Object.Flag)
                )
                .Read(new[] {"--xFlag"});

            mock.Verify(m => m.Flag());
        }

        [Fact]
        public void TestParams_ShouldNotSetFlagValue_IfNotContainedInArgs()
        {
            var mock = new Mock<IArgMock>(MockBehavior.Strict);

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterFlag("yFlag", "y", "", mock.Object.Flag)
                )
                .Read(new string[] { });
        }

        [Fact]
        public void TestParams_ShouldSetMultipleFlags_IfContainedInArgs()
        {
            var mock1 = new Mock<IArgMock>();
            var mock2 = new Mock<IArgMock>();

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterFlag("xFlag1", "x1", "", mock1.Object.Flag)
                    .RegisterFlag("xFlag2", "x2", "", mock2.Object.Flag)
                )
                .Read(new[] {"--xFlag1", "--xFlag2"});

            mock1.Verify(m => m.Flag());
            mock2.Verify(m => m.Flag());
        }

        [Fact]
        public void TestParams_ShouldSetFlag_IfAbrContainedInArgs()
        {
            var mock = new Mock<IArgMock>();

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterFlag("xFlag", "x", "", mock.Object.Flag)
                )
                .Read(new[] {"-x"});

            mock.Verify(m => m.Flag());
        }

        [Fact]
        public void TestParams_ShouldSetMultipleFlags_IfAbrContainedInArgs()
        {
            var mock1 = new Mock<IArgMock>();
            var mock2 = new Mock<IArgMock>();

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "f1", "", mock1.Object.Flag)
                    .RegisterFlag("flag2", "f2", "", mock2.Object.Flag)
                )
                .Read(new[] {"-f1", "-f2"});

            mock1.Verify(m => m.Flag());
            mock2.Verify(m => m.Flag());
        }

        [Fact]
        public void TestParams_ShouldSetMultipleFlags_IfFlagHasNoAbrAndContainedInArgs()
        {
            var mock1 = new Mock<IArgMock>();
            var mock2 = new Mock<IArgMock>();

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "", mock1.Object.Flag)
                    .RegisterFlag("flag2", "", mock2.Object.Flag)
                )
                .Read(new[] {"--flag1", "--flag2"});

            mock1.Verify(m => m.Flag());
            mock2.Verify(m => m.Flag());
        }

        [Fact]
        public void TestParams_ShouldSetFirstParam_IfContainedInArgs()
        {
            var mock = new Mock<IArgMock>();

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterParam("param", "", mock.Object.Param)
                )
                .Read(new[] {"paramValue"});

            mock.Verify(m => m.Param("paramValue"));
        }

        [Fact]
        public void TestParams_ShouldSetSecondParam_IfContainedInArgs()
        {
            var mock1 = new Mock<IArgMock>();
            var mock2 = new Mock<IArgMock>();

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterParam("param1", "", mock1.Object.Param)
                    .RegisterParam("param2", "", mock2.Object.Param)
                )
                .Read(new[] {"paramValue1", "paramValue2"});

            mock1.Verify(m => m.Param("paramValue1"));
            mock2.Verify(m => m.Param("paramValue2"));
        }

        [Fact]
        public void TestParams_ShouldSetSecondFlagsOptionsAndParams()
        {
            var flagMock1 = new Mock<IArgMock>();
            var flagMock2 = new Mock<IArgMock>();
            var optionMock1 = new Mock<IArgMock>();
            var optionMock2 = new Mock<IArgMock>();
            var paramMock1 = new Mock<IArgMock>();
            var paramMock2 = new Mock<IArgMock>();

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "f1", "", flagMock1.Object.Flag)
                    .RegisterFlag("flag2", "", flagMock2.Object.Flag)
                    .RegisterOption("option1", "", optionMock1.Object.Option)
                    .RegisterOption("option2", "o2", "", optionMock2.Object.Option)
                    .RegisterParam("param1", "", paramMock1.Object.Param)
                    .RegisterParam("param2", "", paramMock2.Object.Param)
                )
                .Read(new[]
                {
                    "-f1",
                    "--flag2",
                    "--option1", "optionValue1",
                    "-o2", "optionValue2",
                    "paramValue1",
                    "paramValue2"
                });

            flagMock1.Verify(m => m.Flag());
            flagMock2.Verify(m => m.Flag());
            optionMock1.Verify(m => m.Option("optionValue1"));
            optionMock2.Verify(m => m.Option("optionValue2"));
            paramMock1.Verify(m => m.Param("paramValue1"));
            paramMock2.Verify(m => m.Param("paramValue2"));
        }
    }
}