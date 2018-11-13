using NFlags.Commands;
using Xunit;
using NFAssert = NFlags.Tests.Helpers.Assert;

namespace NFlags.Tests
{
    public class DefaultCommandExecute
    {
        [Fact]
        public void TestParam_ShouldRunDefaultCommandWithParams()
        {
            CommandArgs a = null;
            NFlags
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
        public void TestParam_ShouldRunDefaultCommandWithDefaults()
        {
            CommandArgs a = null;
            NFlags
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
            NFlags
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