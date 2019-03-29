using NFlags.Commands;
using Xunit;

namespace NFlags.Tests
{
    public class ArraySupportTest
    {
        [Fact]
        public void RegisterCommand_ReadMultipleParamsIntoArray_WhenAllowMultiple()
        {
            CommandArgs a = null;

            NFlags
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
        public void RegisterCommand_ShouldUseLastValue_WhenOptionMultipleTimesPassedAndIsNotAnArray()
        {
            CommandArgs a = null;

            NFlags
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
    }
}