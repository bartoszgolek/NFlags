using NFlags.Tests.DataTypes;
using NFlags.Tests.TestImplementations;
using Xunit;

namespace NFlags.Tests
{
    public class GenericValuesPrecedence
    {
        [Fact]
        public void RegisterCommand_ShouldPassCliArgumentsToExecute_WhenDefaultConfigEnvironmentAndCliAreDefined()
        {
            var testEnvironment = new TestEnvironment()
                .SetEnvironmentVariable("NFLAG_TEST_OPTION_ENV", "env_o")
                .SetEnvironmentVariable("NFLAG_TEST_PARAM_ENV", "env_p")
                .SetEnvironmentVariable("NFLAG_TEST_FLAG_ENV", "true");

            var testConfig = new TestConfig()
                .SetConfigValue("OPTION", "conf_o")
                .SetConfigValue("PARAM", "conf_p")
                .SetConfigValue("FLAG", "true");

            var commandArgs = RunTest(testEnvironment, testConfig, new [] {
                "--option",
                "o",
                "--flag",
                "param"
            });

            Assert.Equal("o", commandArgs.Option);
            Assert.Equal("param", commandArgs.Parameter);
            Assert.False(commandArgs.Flag);
        }

        [Fact]
        public void RegisterCommand_ShouldPassEnvironmentValueToExecute_WhenDefaultConfigEnvironmentAndCliAreDefinedAndCliIsNotPassed()
        {
            var testEnvironment = new TestEnvironment()
                .SetEnvironmentVariable("NFLAG_TEST_OPTION_ENV", "env_o")
                .SetEnvironmentVariable("NFLAG_TEST_PARAM_ENV", "env_p")
                .SetEnvironmentVariable("NFLAG_TEST_FLAG_ENV", "false");

            var testConfig = new TestConfig()
                .SetConfigValue("OPTION", "conf_o")
                .SetConfigValue("PARAM", "conf_p")
                .SetConfigValue("FLAG", "true");

            var commandArgs = RunTest(testEnvironment, testConfig, new string[0]);

            Assert.Equal("env_o", commandArgs.Option);
            Assert.Equal("env_p", commandArgs.Parameter);
            Assert.False(commandArgs.Flag);
        }

        [Fact]
        public void RegisterCommand_ShouldPassConfigValueToExecute_WhenDefaultConfigEnvironmentAndCliAreDefinedAndCliIsNotPassedAndEnvVariableIsNotDefined()
        {
            var testEnvironment = new TestEnvironment();

            var testConfig = new TestConfig()
                .SetConfigValue("OPTION", "conf_o")
                .SetConfigValue("PARAM", "conf_p")
                .SetConfigValue("FLAG", "false");

            var commandArgs = RunTest(testEnvironment, testConfig, new string[0]);

            Assert.Equal("conf_o", commandArgs.Option);
            Assert.Equal("conf_p", commandArgs.Parameter);
            Assert.False(commandArgs.Flag);
        }

        [Fact]
        public void RegisterCommand_ShouldPassConfigValueToExecute_WhenDefaultConfigEnvironmentAndCliAreDefinedAndCliIsNotPassedAndEnvVariableAndConfigValueAreNotDefined()
        {
            var testEnvironment = new TestEnvironment();

            var testConfig = new TestConfig();

            var commandArgs = RunTest(testEnvironment, testConfig, new string[0]);

            Assert.Equal("def_o", commandArgs.Option);
            Assert.Equal("def_p", commandArgs.Parameter);
            Assert.True(commandArgs.Flag);
        }

        private static ValuePrecedence RunTest(IEnvironment testEnvironment, IConfig testConfig, string[] cliArgs)
        {
            ValuePrecedence a = null;

            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetEnvironment(testEnvironment)
                    .SetConfiguration(testConfig)
                )
                .Root<ValuePrecedence>(c => c
                    .SetExecute((args, output) => { a = args; })
                )
                .Run(cliArgs);

            return a;
        }
    }
}