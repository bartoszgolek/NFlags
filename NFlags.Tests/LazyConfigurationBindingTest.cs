using NFlags.Commands;
using NFlags.Tests.TestImplementations;
using Xunit;

namespace NFlags.Tests
{
    public class LazyConfigurationBindingTest
    {
        [Fact]
        public void RegisterCommand_ShouldReadFlagLazyConfigPath_IfSetAfterInitialization()
        {
            var testConfig = new TestConfig();

            CommandArgs commandArgs = null;
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetConfiguration(testConfig)
                )
                .Root(c => c
                    .RegisterFlag(b =>
                        b.Name("flag").DefaultValue(true).LazyConfigPath("config.flag"))
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testConfig
                .SetConfigValue("config.flag", "false");

            Assert.False(commandArgs.GetFlag("flag"));
        }

        [Fact]
        public void RegisterCommand_ShouldReadLazyOptionConfigPathValue_IfSetAfterInitialization()
        {
            var testConfig = new TestConfig();

            CommandArgs commandArgs = null;
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetConfiguration(testConfig)
                )
                .Root(c => c
                    .RegisterOption<string>(b =>
                        b.Name("option").DefaultValue("def_o").LazyConfigPath("config.option"))
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testConfig
                .SetConfigValue("config.option", "env_o");

            Assert.Equal("env_o", commandArgs.GetOption<string>("option"));
        }

        [Fact]
        public void RegisterCommand_ShouldReadLazyParameterConfigPathValue_IfSetAfterInitialization()
        {
            var testConfig = new TestConfig();

            CommandArgs commandArgs = null;
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetConfiguration(testConfig)
                )
                .Root(c => c
                    .RegisterParameter<string>(b =>
                        b.Name("parameter").DefaultValue("def_p").LazyConfigPath("config.parameter"))
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testConfig
                .SetConfigValue("config.parameter", "env_p");

            Assert.Equal("env_p", commandArgs.GetParameter<string>("parameter"));
        }

        [Fact]
        public void RegisterCommand_ShouldNotReadFlagConfigPathValue_IfSetAfterInitialization()
        {
            var testConfig = new TestConfig();

            CommandArgs commandArgs = null;
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetConfiguration(testConfig)
                )
                .Root(c => c
                    .RegisterFlag(b =>
                        b.Name("flag").DefaultValue(true).ConfigPath("config.flag"))
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testConfig
                .SetConfigValue("config.flag", "false");

            Assert.True(commandArgs.GetFlag("flag"));
        }

        [Fact]
        public void RegisterCommand_ShouldNotReadOptionConfigPathValue_IfSetAfterInitialization()
        {
            var testConfig = new TestConfig();

            CommandArgs commandArgs = null;
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetConfiguration(testConfig)
                )
                .Root(c => c
                    .RegisterOption<string>(b =>
                        b.Name("option").DefaultValue("def_o").ConfigPath("config.option"))
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testConfig
                .SetConfigValue("config.option", "env_o");

            Assert.Equal("def_o", commandArgs.GetOption<string>("option"));
        }

        [Fact]
        public void RegisterCommand_ShouldNotReadParameterConfigPathValue_IfSetAfterInitialization()
        {
            var testConfig = new TestConfig();

            CommandArgs commandArgs = null;
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetConfiguration(testConfig)
                )
                .Root(c => c
                    .RegisterParameter<string>(b =>
                        b.Name("parameter").DefaultValue("def_p").ConfigPath("config.parameter"))
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testConfig
                .SetConfigValue("config.parameter", "env_p");

            Assert.Equal("def_p", commandArgs.GetParameter<string>("parameter"));
        }
    }
}