using System.Text;
using NFlags.Commands;
using NFlags.Tests.DataTypes;
using NFlags.TypeConverters;
using Xunit;

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

            Assert.Equal(a.Options["option"], 1);
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

            Assert.Equal(a.Options["option"], 2);
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

            Assert.Equal("b", ((TypeWithConstructor)a.Options["option"]).S);
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

            Assert.Equal("b", ((TypeWithImplicitOperator)a.Options["option"]).S);
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

            Assert.Equal(a.Options["option"], 1);
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

            Assert.Equal(a.Options["option"], 2);
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

            var expectedResultBuilder = new StringBuilder();
            expectedResultBuilder.AppendLine("Cannot convert value 'asd' to type 'System.Int32'");
            expectedResultBuilder.AppendLine();
            expectedResultBuilder.AppendLine("Usage:");
            expectedResultBuilder.AppendLine("\ttesthost [FLAGS]... [OPTIONS]...");
            expectedResultBuilder.AppendLine();
            expectedResultBuilder.AppendLine("\tFlags:");
            expectedResultBuilder.AppendLine("\t/help, /h	Prints this help");
            expectedResultBuilder.AppendLine();
            expectedResultBuilder.AppendLine("\tOptions:");
            expectedResultBuilder.AppendLine("\t/option=<option>\t");
            expectedResultBuilder.AppendLine();

            Assert.Equal(expectedResultBuilder.ToString(), outputAggregator.ToString());
        }
    }
}