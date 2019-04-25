using NFlags.Commands;
using NFlags.Tests.TestImplementations;
using Xunit;

namespace NFlags.Tests
{
    public class LazyEnvironmentBindingTest
    {
        [Fact]
        public void RegisterCommand_ShouldReadFlagLazyEnvironmentVariable_IfSetAfterInitialization()
        {
            var testEnvironment = new TestEnvironment();

            CommandArgs commandArgs = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetEnvironment(testEnvironment)
                )
                .Root(c => c
                    .RegisterFlag(b =>
                        b.Name("flag").DefaultValue(true).LazyEnvironmentVariable("NFLAG_TEST_FLAG_ENV"))
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testEnvironment
                .SetEnvironmentVariable("NFLAG_TEST_FLAG_ENV", "false");

            Assert.False(commandArgs.GetFlag("flag"));
        }

        [Fact]
        public void RegisterCommand_ShouldReadLazyOptionEnvironmentVariable_IfSetAfterInitialization()
        {
            var testEnvironment = new TestEnvironment();

            CommandArgs commandArgs = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetEnvironment(testEnvironment)
                )
                .Root(c => c
                    .RegisterOption<string>(b =>
                        b.Name("option").DefaultValue("def_o").LazyEnvironmentVariable("NFLAG_TEST_OPTION_ENV"))
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testEnvironment
                .SetEnvironmentVariable("NFLAG_TEST_OPTION_ENV", "env_o");

            Assert.Equal("env_o", commandArgs.GetOption<string>("option"));
        }

        [Fact]
        public void RegisterCommand_ShouldReadLazyParameterEnvironmentVariable_IfSetAfterInitialization()
        {
            var testEnvironment = new TestEnvironment();

            CommandArgs commandArgs = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetEnvironment(testEnvironment)
                )
                .Root(c => c
                    .RegisterParameter<string>(b =>
                        b.Name("parameter").DefaultValue("def_p").LazyEnvironmentVariable("NFLAG_TEST_PARAM_ENV"))
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testEnvironment
                .SetEnvironmentVariable("NFLAG_TEST_PARAM_ENV", "env_p");

            Assert.Equal("env_p", commandArgs.GetParameter<string>("parameter"));
        }

        [Fact]
        public void RegisterCommand_ShouldNotReadFlagEnvironmentVariable_IfSetAfterInitialization()
        {
            var testEnvironment = new TestEnvironment();

            CommandArgs commandArgs = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetEnvironment(testEnvironment)
                )
                .Root(c => c
                    .RegisterFlag(b =>
                        b.Name("flag").DefaultValue(true).EnvironmentVariable("NFLAG_TEST_FLAG_ENV"))
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testEnvironment
                .SetEnvironmentVariable("NFLAG_TEST_FLAG_ENV", "false");

            Assert.True(commandArgs.GetFlag("flag"));
        }

        [Fact]
        public void RegisterCommand_ShouldNotReadOptionEnvironmentVariable_IfSetAfterInitialization()
        {
            var testEnvironment = new TestEnvironment();

            CommandArgs commandArgs = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetEnvironment(testEnvironment)
                )
                .Root(c => c
                    .RegisterOption<string>(b =>
                        b.Name("option").DefaultValue("def_o").EnvironmentVariable("NFLAG_TEST_OPTION_ENV"))
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testEnvironment
                .SetEnvironmentVariable("NFLAG_TEST_OPTION_ENV", "env_o");

            Assert.Equal("def_o", commandArgs.GetOption<string>("option"));
        }

        [Fact]
        public void RegisterCommand_ShouldNotReadParameterEnvironmentVariable_IfSetAfterInitialization()
        {
            var testEnvironment = new TestEnvironment();

            CommandArgs commandArgs = null;
            Cli
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetEnvironment(testEnvironment)
                )
                .Root(c => c
                    .RegisterParameter<string>(b =>
                        b.Name("parameter").DefaultValue("def_p").EnvironmentVariable("NFLAG_TEST_PARAM_ENV"))
                    .SetExecute((args, output) => { commandArgs = args; })
                )
                .Run(new string[0]);

            testEnvironment
                .SetEnvironmentVariable("NFLAG_TEST_PARAM_ENV", "env_p");

            Assert.Equal("def_p", commandArgs.GetParameter<string>("parameter"));
        }
    }
}