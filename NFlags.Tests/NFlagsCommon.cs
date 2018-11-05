using System;
using System.Text;
using Xunit;
using NFAssert = NFlags.Tests.Helpers.Assert;

namespace NFlags.Tests
{
    public class NFlagsCommon
    {
        [Fact]
        public void NFlags_ShouldThrowException_IfTooManyArgumentsAndExceptionHandlingIsDisabled()
        {
            Assert.Throws<TooManyParametersException>(() =>
                NFlags.Configure(c => c.DisableExceptionHandling())
                    .Root(c => { })
                    .Run(new[] {"s"})
                );
        }

        [Fact]
        public void NFlags_ShouldReturnErrorExitCode_IfTooManyArguments()
        {
            const int errorExitCode = 255;
            var exitCode = NFlags.Configure(c => { })
                .Root(c => { })
                .Run(new[] {"s"});

            Assert.Equal(errorExitCode, exitCode);
        }

        [Fact]
        public void TestParams_ShouldPrintMessageWithHelp_IfCannotConvertValue()
        {
            var outputAggregator = new OutputAggregator();
            NFlags.Configure(c => c
                    .SetOutput(outputAggregator)
                )
                .Root(c => { })
                .Run(new[] {"s"});

            var expectedResultBuilder = new StringBuilder();
            expectedResultBuilder.AppendLine("Two many parameters. Can't handle s value.");
            expectedResultBuilder.AppendLine();
            expectedResultBuilder.AppendLine("Usage:");
            expectedResultBuilder.AppendLine("\ttesthost [FLAGS]...");
            expectedResultBuilder.AppendLine();
            expectedResultBuilder.AppendLine("\tFlags:");
            expectedResultBuilder.AppendLine("\t/help, /h	Prints this help");
            expectedResultBuilder.AppendLine();

            Assert.Equal(expectedResultBuilder.ToString(), outputAggregator.ToString());
        }

        [Fact]
        public void NFlags_ShouldRunRootCommand()
        {
            var rootCmdCalled = false;

            NFlags.Configure(c => { })
                .Root(c => c.SetExecute((args, output) =>
                {
                    rootCmdCalled = true;
                    return 0;
                }))
                .Run(Array.Empty<string>());

            Assert.True(rootCmdCalled);
        }

        [Fact]
        public void NFlags_ShouldRunSubCommand()
        {
            var subCmdCalled = false;

            NFlags.Configure(c => { })
                .Root(c => c.
                    RegisterSubcommand("sub", "desc", sc => sc.
                        SetExecute((args, output) =>
                        {
                            subCmdCalled = true;
                            return 0;
                        })
                    )
                )
                .Run(new[] { "sub" });

            Assert.True(subCmdCalled);
        }

        [Fact]
        public void NFlags_ShouldRunNthLevelSubCommand()
        {
            var subCmdCalled = false;

            NFlags.Configure(c => { })
                .Root(c => c.
                    RegisterSubcommand("sub", "desc", sc => sc.
                        RegisterSubcommand("sub1", "desc1", sc1 => sc1.
                            RegisterSubcommand("sub2", "desc2", sc2 => sc2.
                                RegisterSubcommand("sub3", "desc3", sc3 => sc3.
                                    SetExecute((args, output) =>
                                    {
                                        subCmdCalled = true;
                                        return 0;
                                    })
                                )
                            )
                        )
                    )
                )
                .Run(new[] { "sub", "sub1", "sub2", "sub3" });

            Assert.True(subCmdCalled);
        }

        [Fact]
        public void NFlags_ShouldRunNthLevelSubCommand_EvenIfDeeperCommandsExists()
        {
            var sub2CmdCalled = false;

            NFlags.Configure(c => { })
                .Root(c => c.
                    RegisterSubcommand("sub", "desc", sc => sc.
                        RegisterSubcommand("sub1", "desc1", sc1 => sc1.
                            RegisterSubcommand("sub2", "desc2", sc2 => sc2.
                                RegisterSubcommand("sub3", "desc3",
                                    sc3 => { }
                                )
                                .SetExecute((args, output) =>
                                {
                                    sub2CmdCalled = true;
                                    return 0;
                                })
                            )
                        )
                    )
                )
                .Run(new[] { "sub", "sub1", "sub2" });

            Assert.True(sub2CmdCalled);
        }

        [Fact]
        public void NFlags_WithoutCommandConfigured_ShouldPrintNothing()
        {
            const string expectedOutput = "";

            var outputAggregator = new OutputAggregator();
            NFlags.Configure(configurator => configurator
                    .SetOutput(outputAggregator)
                )
                .Root(c => c
                    .SetExecute((args, output) => { })
                )
                .Run(Array.Empty<string>());

            Assert.Equal(expectedOutput, outputAggregator.ToString());
        }

        [Fact]
        public void TestParams_ShouldPrintSubCommandHelp_IfSubCommandCalled()
        {
            var outputAggregator = new OutputAggregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                    .SetOutput(outputAggregator)
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "f1", "", false)
                    .RegisterFlag("flag2", "", false)
                    .RegisterSubcommand("sub", "subcommand descition", c => c
                        .RegisterOption("option1", "", "")
                        .RegisterOption("option2", "o2", "", "")
                        .RegisterParam("param1", "", "")
                        .RegisterParam("param2", "", "")
                    )
                )
                .Run(new[]
                {
                    "sub",
                    "/h"
                });

            NFAssert.HelpEquals(
                outputAggregator.ToString(),
                "Usage:",
               "\ttesthost sub [FLAGS]... [OPTIONS]... [PARAMETERS]...",
               "",
               "\tFlags:",
               "\t/help, /h\tPrints this help",
               "",
               "\tOptions:",
               "\t/option1=<option1>",
               "\t/option2=<option2>, /o2=<option2>",
               "",
               "\tParameters:",
               "\t<param1>",
               "\t<param2>",
               ""
            );
        }

        [Fact]
        public void NFlags_ShouldPrintHelpForNthLevelSubCommand()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost sub sub1 sub2 sub3 [FLAGS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "\tFlags:" + Environment.NewLine +
                               "\t/help, /h\tPrints this help" + Environment.NewLine +
                               Environment.NewLine;

            var outputAggregator = new OutputAggregator();
            NFlags.Configure(c => c.SetOutput(outputAggregator))
                .Root(c => c.
                    RegisterSubcommand("sub", "desc", sc => sc.
                        RegisterSubcommand("sub1", "desc1", sc1 => sc1.
                            RegisterSubcommand("sub2", "desc2", sc2 => sc2.
                                RegisterSubcommand("sub3", "desc3", sc3 => { })
                            )
                        )
                    )
                )
                .Run(new[] { "sub", "sub1", "sub2", "sub3", "/h"});

            Assert.Equal(expectedHelp, outputAggregator.ToString());
        }

        [Fact]
        public void NFlags_ShouldPrintHelpForPersistentFlags()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost [FLAGS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "\tFlags:" + Environment.NewLine +
                               "\t/flag1, /f1\tdFlag1" + Environment.NewLine +
                               "\t/flag2\tdFlag2" + Environment.NewLine +
                               "\t/help, /h\tPrints this help" + Environment.NewLine +
                               Environment.NewLine;

            var outputAggregator = new OutputAggregator();
            NFlags.Configure(c => c.SetOutput(outputAggregator))
                .Root(c => c.
                    RegisterPersistentFlag("flag1", "f1", "dFlag1", false).
                    RegisterPersistentFlag("flag2", "dFlag2", false)
                )
                .Run(new[] { "/h"});

            Assert.Equal(expectedHelp, outputAggregator.ToString());
        }

        [Fact]
        public void NFlags_ShouldPrintHelpForPersistentFlagsAtNthLevelSubCommand()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost sub sub1 sub2 sub3 [FLAGS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "\tFlags:" + Environment.NewLine +
                               "\t/flag1, /f1\tdFlag1" + Environment.NewLine +
                               "\t/flag2\tdFlag2" + Environment.NewLine +
                               "\t/help, /h\tPrints this help" + Environment.NewLine +
                               Environment.NewLine;

            var outputAggregator = new OutputAggregator();
            NFlags.Configure(c => c.SetOutput(outputAggregator))
                .Root(c => c.
                    RegisterPersistentFlag("flag1", "f1", "dFlag1", false).
                    RegisterSubcommand("sub", "desc", sc => sc.
                        RegisterSubcommand("sub1", "desc1", sc1 => sc1.
                            RegisterSubcommand("sub2", "desc2", sc2 => sc2.
                                RegisterSubcommand("sub3", "desc3", sc3 => { })
                            )
                        )
                    ).
                    RegisterPersistentFlag("flag2", "dFlag2", false)
                )
                .Run(new[] { "sub", "sub1", "sub2", "sub3", "/h"});

            Assert.Equal(expectedHelp, outputAggregator.ToString());
        }

        [Fact]
        public void NFlags_ShouldPrintHelpForPersistentOptions()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost [FLAGS]... [OPTIONS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "\tFlags:" + Environment.NewLine +
                               "\t/help, /h\tPrints this help" + Environment.NewLine +
                               Environment.NewLine +
                               "\tOptions:" + Environment.NewLine +
                               "\t/option1=<option1>, /o1=<option1>\tdOption1" + Environment.NewLine +
                               "\t/option2=<option2>\tdOption2" + Environment.NewLine +
                               Environment.NewLine;

            var outputAggregator = new OutputAggregator();
            NFlags.Configure(c => c.SetOutput(outputAggregator))
                .Root(c => c.
                    RegisterPersistentOption("option1", "o1", "dOption1", "").
                    RegisterPersistentOption("option2", "dOption2", "")
                )
                .Run(new[] { "/h"});

            Assert.Equal(expectedHelp, outputAggregator.ToString());
        }

        [Fact]
        public void NFlags_ShouldPrintHelpForPersistentOptionsAtNthLevelSubCommand()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost sub sub1 sub2 sub3 [FLAGS]... [OPTIONS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "\tFlags:" + Environment.NewLine +
                               "\t/help, /h\tPrints this help" + Environment.NewLine +
                               Environment.NewLine +
                               "\tOptions:" + Environment.NewLine +
                               "\t/option1=<option1>, /o1=<option1>\tdOption1" + Environment.NewLine +
                               "\t/option2=<option2>\tdOption2" + Environment.NewLine +
                               Environment.NewLine;

            var outputAggregator = new OutputAggregator();
            NFlags.Configure(c => c.SetOutput(outputAggregator))
                .Root(c => c.
                    RegisterPersistentOption("option1", "o1", "dOption1", "").
                    RegisterSubcommand("sub", "desc", sc => sc.
                        RegisterSubcommand("sub1", "desc1", sc1 => sc1.
                            RegisterSubcommand("sub2", "desc2", sc2 => sc2.
                                RegisterSubcommand("sub3", "desc3", sc3 => { })
                            )
                        )
                    ).
                    RegisterPersistentOption("option2", "dOption2", "")
                )
                .Run(new[] { "sub", "sub1", "sub2", "sub3", "/h"});

            Assert.Equal(expectedHelp, outputAggregator.ToString());
        }
    }
}