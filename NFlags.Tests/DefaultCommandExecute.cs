using NFlags.Commands;
using Xunit;
using NFAssert = NFlags.Tests.Helpers.Assert;

namespace NFlags.Tests
{
    public class DefaultCommandExecute
    {
        [Fact]
        public void TestCommandConfigurator_ShouldThrowTooManyDefaultCommandsException_IfTryingToRegisterMultipleDefaultCommands()
        {
            Assert.Throws<TooManyDefaultCommandsException>(() =>
            {
                Cli
                    .Configure(c => { })
                    .Root(c => c
                        .RegisterDefaultCommand("defaultCommand", "defaultCommandDescription", dc => { })
                        .RegisterDefaultCommand("defaultCommand2", "defaultCommandDescription", dc => { })
                    )
                    .Run(new[] {"ff"});
            });
        }

        [Fact]
        public void TestRun_ShouldRunDefaultCommandWithParams()
        {
            CommandArgs a = null;
            Cli
                .Configure(c => { })
                .Root(c => c
                    .RegisterDefaultCommand("defaultCommand", "defaultCommandDescription", dc => dc
                        .RegisterParameter("param1", "paramDesc", "xx")
                        .SetExecute((args, output) =>
                        {
                            a = args;
                        })
                    ))
                .Run(new []{ "ff" });

                Assert.Equal("ff", a.GetParameter<string>("param1"));            
        }

        [Fact]
        public void TestRun_ShouldRunDefaultCommandWithDefaults()
        {
            CommandArgs a = null;
            Cli
                .Configure(c => { })
                .Root(c => c
                    .RegisterDefaultCommand("defaultCommand", "defaultCommandDescription", dc => dc
                        .RegisterParameter("param1", "paramDesc", "xx")
                        .SetExecute((args, output) =>
                        {
                            a = args;
                        })
                    ))
                .Run(new string[0]);

                Assert.Equal("xx", a.GetParameter<string>("param1"));            
        }

        [Fact]
        public void TestParam_ShouldRunDefaultOfDefaultCommand()
        {
            CommandArgs a = null;
            Cli
                .Configure(c => { })
                .Root(c => c
                    .RegisterDefaultCommand("defaultCommand", "defaultCommandDescription", dc => dc
                        .RegisterParameter("param1", "param Desc", "sx")
                        .RegisterDefaultCommand("defaultCommand2", "defaultCommandDescription", dc2 => dc2
                            .RegisterParameter("param2", "param2Desc", "xx")
                            .SetExecute((args, output) =>
                            {
                                a = args;
                            })
                        )
                    )
                )
                .Run(new string[0]);

                Assert.Equal("xx", a.GetParameter<string>("param2"));            
        }
    }
}