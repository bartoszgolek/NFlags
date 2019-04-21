using NFlags.Commands;
using NFlags.Tests.DataTypes;
using NFlags.Tests.TestImplementations;
using Xunit;

namespace NFlags.Tests
{
    public class ArraySupportTest
    {
        [Fact]
        public void RegisterCommand_ReadMultipleParamsIntoArray_WhenAllowMultiple()
        {
            CommandArgs a = null;

            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .DisableExceptionHandling())
                .Root(c => c
                    .RegisterOption<int[]>(b =>
                        b.Name("int-array")
                    )
                    .SetExecute((args, output) => a = args )
                ).Run(new[]
                {
                    "--int-array", "1",
                    "--int-array", "2",
                    "--int-array", "3"
                });

            Assert.Equal(new[]{ 1, 2, 3}, a.GetOption<int[]>("int-array"));
        }

        [Fact]
        public void RegisterCommandGeneric_ReadMultipleParamsIntoArray_WhenAllowMultiple()
        {
            ArrayArgumentType a = null;

            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .DisableExceptionHandling())
                .Root<ArrayArgumentType>(c => c.SetExecute((args, output) => a = args )
                ).Run(new[]
                {
                    "--int-array", "1",
                    "--int-array", "2",
                    "--int-array", "3"
                });

            Assert.Equal(new[]{ 1, 2, 3}, a.Option);
        }

        [Fact]
        public void RegisterCommand_ShouldUseLastValue_WhenOptionMultipleTimesPassedAndIsNotAnArray()
        {
            CommandArgs a = null;

            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .DisableExceptionHandling())
                .Root(c => c
                    .RegisterOption<int>(b =>
                        b.Name("int-array")
                    )
                    .SetExecute((args, output) => a = args )
                ).Run(new[]
                {
                    "--int-array", "1",
                    "--int-array", "2",
                    "--int-array", "3"
                });

            Assert.Equal(3, a.GetOption<int>("int-array"));
        }

        [Fact]
        public void RegisterCommand_ReadConfigValueIntoArray()
        {
            CommandArgs a = null;

            var testConfig = new TestConfig();
            testConfig.SetConfigValue("Test:Array", "1;2;3");

            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetConfiguration(testConfig)
                    .DisableExceptionHandling())
                .Root(c => c
                    .RegisterOption<int[]>(b =>
                        b.Name("int-array").ConfigPath("Test:Array")
                    )
                    .SetExecute((args, output) => a = args )
                ).Run(new string[0]);

            Assert.Equal(new[]{ 1, 2, 3}, a.GetOption<int[]>("int-array"));
        }
    }
}