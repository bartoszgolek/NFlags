using NFlags.Tests.DataTypes;
using NFlags.Tests.TestImplementations;
using Xunit;

namespace NFlags.Tests
{
    public class GenericLazyEnvironmentBindingTest
    {
        [Fact]
        public void RegisterCommandT_ShouldReadFlagLazyEnvironmentVariable_IfSetAfterInitialization()
        {
            var testEnvironment = new TestEnvironment();

            LazyEnvironmentArgumentsType commandArgs = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetEnvironment(testEnvironment)
                )
                .Root<LazyEnvironmentArgumentsType>(c => c
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testEnvironment
                .SetEnvironmentVariable("NFLAG_TEST_FLAG_ENV", "false");

            Assert.False(commandArgs.Flag.Value);
        }

        [Fact]
        public void RegisterCommandT_ShouldReadLazyOptionEnvironmentVariable_IfSetAfterInitialization()
        {
            var testEnvironment = new TestEnvironment();

            LazyEnvironmentArgumentsType commandArgs = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetEnvironment(testEnvironment)
                )
                .Root<LazyEnvironmentArgumentsType>(c => c
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testEnvironment
                .SetEnvironmentVariable("NFLAG_TEST_OPTION_ENV", "env_o");

            Assert.Equal("env_o", commandArgs.Option.Value);
        }

        [Fact]
        public void RegisterCommandT_ShouldReadLazyParameterEnvironmentVariable_IfSetAfterInitialization()
        {
            var testEnvironment = new TestEnvironment();

            LazyEnvironmentArgumentsType commandArgs = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetEnvironment(testEnvironment)
                )
                .Root<LazyEnvironmentArgumentsType>(c => c
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testEnvironment
                .SetEnvironmentVariable("NFLAG_TEST_PARAM_ENV", "env_p");

            Assert.Equal("env_p", commandArgs.Parameter.Value);
        }

        [Fact]
        public void RegisterCommandT_ShouldNotReadFlagEnvironmentVariable_IfSetAfterInitialization()
        {
            var testEnvironment = new TestEnvironment();

            ArgumentsType commandArgs = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetEnvironment(testEnvironment)
                )
                .Root<ArgumentsType>(c => c
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testEnvironment
                .SetEnvironmentVariable("NFLAG_TEST_FLAG1", "false");

            Assert.True(commandArgs.Flag1);
        }

        [Fact]
        public void RegisterCommandT_ShouldNotReadOptionEnvironmentVariable_IfSetAfterInitialization()
        {
            var testEnvironment = new TestEnvironment();

            ArgumentsType commandArgs = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetEnvironment(testEnvironment)
                )
                .Root<ArgumentsType>(c => c
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testEnvironment
                .SetEnvironmentVariable("NFLAG_TEST_OPTION1", "2");

            Assert.Equal(1, commandArgs.Option1);
        }

        [Fact]
        public void RegisterCommandT_ShouldNotReadParameterEnvironmentVariable_IfSetAfterInitialization()
        {
            var testEnvironment = new TestEnvironment();

            ArgumentsType commandArgs = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetEnvironment(testEnvironment)
                )
                .Root<ArgumentsType>(c => c
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testEnvironment
                .SetEnvironmentVariable("NFLAG_TEST_PARAMETER2", "env_p");

            Assert.Equal(1, commandArgs.Parameter2);
        }
    }
}