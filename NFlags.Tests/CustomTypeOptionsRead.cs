using NFlags.Commands;
using NFlags.Tests.DataTypes;
using NFlags.Tests.TestImplementations;
using NFlags.TypeConverters;
using Xunit;
using NFAssert = NFlags.Tests.Helpers.Assert;

namespace NFlags.Tests
{
    public class CustomTypeOptionsType
    {
        private const int ErrorExitCode = 255;

        [Fact]
        public void TestParams_ShouldSetDefaultOptionValueForTypeWhereStringIsConvertibleTo_IfNotContainedInArgs()
        {
            CommandArgs a = null;

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "o", "", 1)
                    .SetExecute((args, output) =>
                    {
                        a = args;
                    })
                )
                .Run(new string[] { });

            Assert.Equal(1, a.GetOption<int>("option"));
        }

        [Fact]
        public void TestParams_ShouldSetOptionValueForTypeWhereStringIsConvertibleTo_IfContainedInArgs()
        {
            CommandArgs a = null;

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "o", "", 1)
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"/option=2"});

            Assert.Equal(2, a.GetOption<int>("option"));
        }

        [Fact]
        public void TestParams_ShouldSetOptionValueForTypeWithConstructorFromString_IfContainedInArgs()
        {
            CommandArgs a = null;

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "o", "", new TypeWithConstructor("a"))
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"/option=b"});

            Assert.Equal("b", a.GetOption<TypeWithConstructor>("option").S);
        }

        [Fact]
        public void TestParams_ShouldSetOptionValueForTypeWithImplicitOperatorFromString_IfContainedInArgs()
        {
            CommandArgs a = null;

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "o", "", new TypeWithImplicitOperator())
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"/option=b"});

            Assert.Equal("b", a.GetOption<TypeWithImplicitOperator>("option").S);
        }

        [Fact]
        public void TestParams_ShouldThrowExceptionForTypeWithExplicitOperatorFromString()
        {
            Assert.Throws<MissingConverterException>(() =>
            {
                NFlags.Configure(configurator => configurator
                        .SetDialect(Dialect.Win)
                    )
                    .Root(configurator => configurator
                        .RegisterOption("option", "o", "", new TypeWithExplicitOperator())
                        .SetExecute((args, output) => 0)
                    );
            });
        }

        [Fact]
        public void TestParams_ShouldSetDefaultNoAbrOptionValueForTypeWhereStringIsConvertibleTo_IfNotContainedInArgs()
        {
            CommandArgs a = null;

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "", 1)
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new string[] { });

            Assert.Equal(1, a.GetOption<int>("option"));
        }

        [Fact]
        public void TestParams_ShouldSetNoAbrOptionValueForTypeWhereStringIsConvertibleTo_IfContainedInArgs()
        {
            CommandArgs a = null;

            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "", 1)
                    .SetExecute((args, output) =>
                    {
                        a = args;
                        return 0;
                    })
                )
                .Run(new[] {"/option=2"});

            Assert.Equal(2, a.GetOption<int>("option"));
        }

        [Fact]
        public void TestParams_ShouldReturnErrorExitCode_IfCannotConvertValue()
        {
            Assert.Equal(ErrorExitCode, NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "", 1)
                    .SetExecute((args, output) => 0)
                )
                .Run(new[] {"/option=asd"}));
        }

        [Fact]
        public void TestParams_ShouldThrowArgumentValueException_IfCannotConvertValueAndExceptionHandlingIsDisabled()
        {
            Assert.Throws<ArgumentValueException>(() =>
                NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                    .DisableExceptionHandling()
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "", 1)
                    .SetExecute((args, output) => { })
                )
                .Run(new[] {"--option", "asd"}));
        }

        [Fact]
        public void TestParams_ShouldPrintMessageWithHelp_IfCannotConvertValue()
        {
            var outputAggregator = new OutputAggregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                    .SetOutput(outputAggregator)
                )
                .Root(configurator => configurator
                    .RegisterOption("option", "", 1)
                    .SetExecute((args, output) => 0)
                )
                .Run(new[] {"/option=asd"});

            NFAssert.HelpEquals(
                outputAggregator,
                "Cannot convert value 'asd' to type 'System.Int32'",
                "",
                "Usage:",
                "\ttesthost [OPTIONS]...",
                "",
                "\tOptions:",
                "\t/option=<option>\t(Default: 1)",
                "\t/help, /h\tPrints this help",
                ""
            );
        }
    }
}