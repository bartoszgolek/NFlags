using System;
using Xunit;

namespace NFlags.Tests
{
    public class GnuDialectPrintHelp
    {
        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithDefaultName()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost" + Environment.NewLine +
                               Environment.NewLine;

            var help = NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root()
                .PrintHelp();

            Assert.Equal(expectedHelp, help);
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithCustomName()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\tcustName" + Environment.NewLine +
                               Environment.NewLine;

            var help = NFlags.Configure(configurator => configurator
                    .SetName("custName")
                    .SetDialect(Dialect.Gnu)
                )
                .Root()
                .PrintHelp();

            Assert.Equal(expectedHelp, help);
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithDescription()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost" + Environment.NewLine +
                               Environment.NewLine +
                               "description asdsd sa" + Environment.NewLine +
                               Environment.NewLine;

            var help = NFlags.Configure(configurator => configurator
                    .SetDescription("description asdsd sa")
                    .SetDialect(Dialect.Gnu)
                )
                .Root()
                .PrintHelp();

            Assert.Equal(expectedHelp, help);
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithFlags()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost [FLAGS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "\tFlags:" + Environment.NewLine +
                               "\t--flag1\tFlag 1 Description" + Environment.NewLine +
                               "\t--flag2, -f2\tFlag 2 Description" + Environment.NewLine +
                               Environment.NewLine;

            var help = NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "Flag 1 Description", false)
                    .RegisterFlag("flag2", "f2", "Flag 2 Description", false)
                )
                .PrintHelp();

            Assert.Equal(expectedHelp, help);
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithOptions()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost [OPTIONS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "\tOptions:" + Environment.NewLine +
                               "\t--option1 <option1>\tOption 1 Description" + Environment.NewLine +
                               "\t--option2 <option2>, -o2 <option2>\tOption 2 Description" + Environment.NewLine +
                               Environment.NewLine;

            var help = NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterOption("option1", "Option 1 Description", "")
                    .RegisterOption("option2", "o2", "Option 2 Description", "")
                )
                .PrintHelp();

            Assert.Equal(expectedHelp, help);
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithParams()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost [PARAMETERS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "\tParameters:" + Environment.NewLine +
                               "\t<param1>\tParam 1 Description" + Environment.NewLine +
                               "\t<param2>\tParam 2 Description" + Environment.NewLine +
                               Environment.NewLine;

            var help = NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                )
                .Root(configurator => configurator
                    .RegisterParam("param1", "Param 1 Description", "")
                    .RegisterParam("param2", "Param 2 Description", "")
                )
                .PrintHelp();

            Assert.Equal(expectedHelp, help);
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithNameDescriptionFlagsOptionsAndParams()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\tcustName [FLAGS]... [OPTIONS]... [PARAMETERS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "description asdsd sa" + Environment.NewLine +
                               Environment.NewLine +
                               "\tFlags:" + Environment.NewLine +
                               "\t--flag1\tFlag 1 Description" + Environment.NewLine +
                               "\t--flag2, -f2\tFlag 2 Description" + Environment.NewLine +
                               Environment.NewLine +
                               "\tOptions:" + Environment.NewLine +
                               "\t--option1 <option1>\tOption 1 Description" + Environment.NewLine +
                               "\t--option2 <option2>, -o2 <option2>\tOption 2 Description" + Environment.NewLine +
                               Environment.NewLine +
                               "\tParameters:" + Environment.NewLine +
                               "\t<param1>\tParam 1 Description" + Environment.NewLine +
                               "\t<param2>\tParam 2 Description" + Environment.NewLine +
                               Environment.NewLine;

            var help = NFlags.Configure(configurator => configurator
                    .SetDialect(Dialect.Gnu)
                    .SetName("custName")
                    .SetDescription("description asdsd sa")
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "Flag 1 Description", false)
                    .RegisterFlag("flag2", "f2", "Flag 2 Description", false)
                    .RegisterOption("option1", "Option 1 Description", "")
                    .RegisterOption("option2", "o2", "Option 2 Description", "")
                    .RegisterParam("param1", "Param 1 Description", "")
                    .RegisterParam("param2", "Param 2 Description", "")
                )
                .PrintHelp();

            Assert.Equal(expectedHelp, help);
        }
    }
}