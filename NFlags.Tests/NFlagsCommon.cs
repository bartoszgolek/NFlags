using System;
using Xunit;

namespace NFlags.Tests
{
    public class NFlagsCommon
    {
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

            var outputAgregator = new OutputAgregator();
            NFlags.Configure(configurator => configurator
                    .SetOutput(outputAgregator)
                )
                .Root(c => { })
                .Run(Array.Empty<string>());

            Assert.Equal(expectedOutput, outputAgregator.ToString());
        }

        [Fact]
        public void TestParams_ShouldPrintSubCommandHelp_IfSubCommandCalled()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                                  "\ttesthost sub [FLAGS]... [OPTIONS]... [PARAMETERS]..." + Environment.NewLine +
                                  Environment.NewLine +
                                  "\tFlags:" + Environment.NewLine +
                                  "\t/help, /h\tPrints this help" + Environment.NewLine +
                                  Environment.NewLine +
                                  "\tOptions:" + Environment.NewLine +
                                  "\t/option1=<option1>\t" + Environment.NewLine +
                                  "\t/option2=<option2>, /o2=<option2>\t" + Environment.NewLine +
                                  Environment.NewLine +
                                  "\tParameters:" + Environment.NewLine +
                                  "\t<param1>\t" + Environment.NewLine +
                                  "\t<param2>\t" + Environment.NewLine +
                                  Environment.NewLine;

            var outputAgregator = new OutputAgregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Win)
                    .SetOutput(outputAgregator)
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

            Assert.Equal(expectedHelp, outputAgregator.ToString());
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

            var outputAgregator = new OutputAgregator();
            NFlags.Configure(c => c.SetOutput(outputAgregator))
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

            Assert.Equal(expectedHelp, outputAgregator.ToString());
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

            var outputAgregator = new OutputAgregator();
            NFlags.Configure(c => c.SetOutput(outputAgregator))
                .Root(c => c.
                    RegisterPersistentFlag("flag1", "f1", "dFlag1", false).
                    RegisterPersistentFlag("flag2", "dFlag2", false)
                )
                .Run(new[] { "/h"});

            Assert.Equal(expectedHelp, outputAgregator.ToString());
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

            var outputAgregator = new OutputAgregator();
            NFlags.Configure(c => c.SetOutput(outputAgregator))
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

            Assert.Equal(expectedHelp, outputAgregator.ToString());
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

            var outputAgregator = new OutputAgregator();
            NFlags.Configure(c => c.SetOutput(outputAgregator))
                .Root(c => c.
                    RegisterPersistentOption("option1", "o1", "dOption1", "").
                    RegisterPersistentOption("option2", "dOption2", "")
                )
                .Run(new[] { "/h"});

            Assert.Equal(expectedHelp, outputAgregator.ToString());
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

            var outputAgregator = new OutputAgregator();
            NFlags.Configure(c => c.SetOutput(outputAgregator))
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

            Assert.Equal(expectedHelp, outputAgregator.ToString());
        }
    }
}