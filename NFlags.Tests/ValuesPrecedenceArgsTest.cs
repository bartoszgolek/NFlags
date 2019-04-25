using NFlags.Commands;
using NFlags.Tests.TestImplementations;
using Xunit;

namespace NFlags.Tests
{
    public class ValuesPrecedenceArgsTest
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

            Assert.Equal("o", commandArgs.GetOption<string>("option"));
            Assert.Equal("param", commandArgs.GetParameter<string>("parameter"));
            Assert.False(commandArgs.GetFlag("flag"));
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

            Assert.Equal("env_o", commandArgs.GetOption<string>("option"));
            Assert.Equal("env_p", commandArgs.GetParameter<string>("parameter"));
            Assert.False(commandArgs.GetFlag("flag"));
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

            Assert.Equal("conf_o", commandArgs.GetOption<string>("option"));
            Assert.Equal("conf_p", commandArgs.GetParameter<string>("parameter"));
            Assert.False(commandArgs.GetFlag("flag"));
        }

        [Fact]
        public void RegisterCommand_ShouldPassConfigValueToExecute_WhenDefaultConfigEnvironmentAndCliAreDefinedAndCliIsNotPassedAndEnvVariableAndConfigValueAreNotDefined()
        {
            var testEnvironment = new TestEnvironment();

            var testConfig = new TestConfig();

            var commandArgs = RunTest(testEnvironment, testConfig, new string[0]);

            Assert.Equal("def_o", commandArgs.GetOption<string>("option"));
            Assert.Equal("def_p", commandArgs.GetParameter<string>("parameter"));
            Assert.True(commandArgs.GetFlag("flag"));
        }

        private static CommandArgs RunTest(IEnvironment testEnvironment, IConfig testConfig, string[] cliArgs)
        {
            CommandArgs a = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetEnvironment(testEnvironment)
                    .SetConfiguration(testConfig)
                    .SetConfiguration(testConfig)
                )
                .Root(c => c
                    .RegisterFlag(b =>
                        b.Name("flag").DefaultValue(true).EnvironmentVariable("NFLAG_TEST_FLAG_ENV").ConfigPath("FLAG"))
                    .RegisterOption<string>(b =>
                        b.Name("option").DefaultValue("def_o").EnvironmentVariable("NFLAG_TEST_OPTION_ENV").ConfigPath("OPTION"))
                    .RegisterParameter<string>(b =>
                        b.Name("parameter").DefaultValue("def_p").EnvironmentVariable("NFLAG_TEST_PARAM_ENV").ConfigPath("PARAM"))
                    .SetExecute((args, output) => { a = args; })
                )
                .Run(cliArgs);
            return a;
        }
    }
}