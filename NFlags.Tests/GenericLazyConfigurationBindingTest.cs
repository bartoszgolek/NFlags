using NFlags.Tests.DataTypes;
using NFlags.Tests.TestImplementations;
using Xunit;

namespace NFlags.Tests
{
    public class GenericLazyConfigurationBindingTest
    {
        [Fact]
        public void RegisterCommandT_ShouldReadFlagLazyEnvironmentVariable_IfSetAfterInitialization()
        {
            var testConfig = new TestConfig();

            LazyConfigPathArgumentsType commandArgs = null;
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetConfiguration(testConfig)
                )
                .Root<LazyConfigPathArgumentsType>(c => c
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testConfig
                .SetConfigValue("config.flag", "false");

            Assert.False(commandArgs.Flag.Value);
        }

        [Fact]
        public void RegisterCommandT_ShouldReadLazyOptionEnvironmentVariable_IfSetAfterInitialization()
        {
            var testConfig = new TestConfig();

            LazyConfigPathArgumentsType commandArgs = null;
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetConfiguration(testConfig)
                )
                .Root<LazyConfigPathArgumentsType>(c => c
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testConfig
                .SetConfigValue("config.option", "env_o");

            Assert.Equal("env_o", commandArgs.Option.Value);
        }

        [Fact]
        public void RegisterCommandT_ShouldReadLazyParameterEnvironmentVariable_IfSetAfterInitialization()
        {
            var testConfig = new TestConfig();

            LazyConfigPathArgumentsType commandArgs = null;
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetConfiguration(testConfig)
                )
                .Root<LazyConfigPathArgumentsType>(c => c
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testConfig
                .SetConfigValue("config.parameter", "env_p");

            Assert.Equal("env_p", commandArgs.Parameter.Value);
        }

        [Fact]
        public void RegisterCommandT_ShouldNotReadFlagEnvironmentVariable_IfSetAfterInitialization()
        {
            var testConfig = new TestConfig();

            NonLazyConfigPathArgumentsType commandArgs = null;
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetConfiguration(testConfig)
                )
                .Root<NonLazyConfigPathArgumentsType>(c => c
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testConfig
                .SetConfigValue("NFLAG_TEST_FLAG1", "false");

            Assert.True(commandArgs.Flag);
        }

        [Fact]
        public void RegisterCommandT_ShouldNotReadOptionEnvironmentVariable_IfSetAfterInitialization()
        {
            var testConfig = new TestConfig();

            NonLazyConfigPathArgumentsType commandArgs = null;
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetConfiguration(testConfig)
                )
                .Root<NonLazyConfigPathArgumentsType>(c => c
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testConfig
                .SetConfigValue("config.option", "2");

            Assert.Equal("def_o", commandArgs.Option);
        }

        [Fact]
        public void RegisterCommandT_ShouldNotReadParameterEnvironmentVariable_IfSetAfterInitialization()
        {
            var testConfig = new TestConfig();

            NonLazyConfigPathArgumentsType commandArgs = null;
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetConfiguration(testConfig)
                )
                .Root<NonLazyConfigPathArgumentsType>(c => c
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testConfig
                .SetConfigValue("config.parameter", "env_p");

            Assert.Equal("def_p", commandArgs.Parameter);
        }
    }
}