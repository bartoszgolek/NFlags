using System;
using Xunit;

namespace NFlags.Tests
{
    public abstract class DialectPrintHelp
    {
        private readonly Dialect _dialect;
        private readonly string _longPrefix;
        private readonly string _shortPrefix;
        private readonly string _optionValueSeparator;

        protected DialectPrintHelp(Dialect dialect, string shortPrefix, string longPrefix, string optionValueSeparator)
        {
            _dialect = dialect;
            _longPrefix = longPrefix;
            _shortPrefix = shortPrefix;
            _optionValueSeparator = optionValueSeparator;
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithDefaultName()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost [FLAGS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "\tFlags:" + Environment.NewLine +
                               "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help" + Environment.NewLine +
                               Environment.NewLine;

            var outputAgregator = new OutputAgregator();
            NFlags.Configure(configurator =>
                {
                    configurator
                        .SetDialect(_dialect)
                        .SetOutput(outputAgregator);
                })
                .Root(c => { })
                .Run(new[] { "" + _longPrefix + "help" });

            Assert.Equal(expectedHelp, outputAgregator.ToString());
        }

        [Fact]
        public void PrintHelp_ShouldPrintSubCommandBasicInfoWithDefaultName()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost [FLAGS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "\tFlags:" + Environment.NewLine +
                               "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help" + Environment.NewLine +
                               Environment.NewLine;

            var outputAgregator = new OutputAgregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAgregator)
                )
                .Root(c => { })
                .Run(new[] { "" + _longPrefix + "help" });

            Assert.Equal(expectedHelp, outputAgregator.ToString());
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithCustomName()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\tcustName [FLAGS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "\tFlags:" + Environment.NewLine +
                               "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help" + Environment.NewLine +
                               Environment.NewLine;

            var outputAgregator = new OutputAgregator();
            NFlags.Configure(configurator => configurator
                    .SetName("custName")
                    .SetDialect(_dialect)
                    .SetOutput(outputAgregator)
                )
                .Root(c => { })
                .Run(new[] { "" + _longPrefix + "help" });

            Assert.Equal(expectedHelp, outputAgregator.ToString());
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithDescription()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost [FLAGS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "some description" + Environment.NewLine +
                               Environment.NewLine +
                               "\tFlags:" + Environment.NewLine +
                               "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help" + Environment.NewLine +
                               Environment.NewLine;

            var outputAgregator = new OutputAgregator();
            NFlags.Configure(configurator => configurator
                    .SetDescription("some description")
                    .SetDialect(_dialect)
                    .SetOutput(outputAgregator)
                )
                .Root(c => { })
                .Run(new[] { "" + _longPrefix + "help" });

            Assert.Equal(expectedHelp, outputAgregator.ToString());
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithFlags()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost [FLAGS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "\tFlags:" + Environment.NewLine +
                               "\t" + _longPrefix + "flag1\tFlag 1 Description" + Environment.NewLine +
                               "\t" + _longPrefix + "flag2, " + _shortPrefix + "f2\tFlag 2 Description" + Environment.NewLine +
                               "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help" + Environment.NewLine +
                               Environment.NewLine;

            var outputAgregator = new OutputAgregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAgregator)
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "Flag 1 Description", false)
                    .RegisterFlag("flag2", "f2", "Flag 2 Description", false)
                )
                .Run(new[] { "" + _longPrefix + "help" });

            Assert.Equal(expectedHelp, outputAgregator.ToString());
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithOptions()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost [FLAGS]... [OPTIONS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "\tFlags:" + Environment.NewLine +                               
                               "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help" + Environment.NewLine +
                               Environment.NewLine +
                               "\tOptions:" + Environment.NewLine +
                               "\t" + _longPrefix + "option1" + _optionValueSeparator + "<option1>\tOption 1 Description" + Environment.NewLine +
                               "\t" + _longPrefix + "option2" + _optionValueSeparator + "<option2>, " + _shortPrefix + "o2" + _optionValueSeparator + "<option2>\tOption 2 Description" + Environment.NewLine +
                               Environment.NewLine;

            var outputAgregator = new OutputAgregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAgregator)
                )
                .Root(configurator => configurator
                    .RegisterOption("option1", "Option 1 Description", "")
                    .RegisterOption("option2", "o2", "Option 2 Description", "")
                )
                .Run(new[] { "" + _longPrefix + "help" });

            Assert.Equal(expectedHelp, outputAgregator.ToString());
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithParams()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost [FLAGS]... [PARAMETERS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "\tFlags:" + Environment.NewLine +                               
                               "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help" + Environment.NewLine +
                               Environment.NewLine +
                               "\tParameters:" + Environment.NewLine +
                               "\t<param1>\tParam 1 Description" + Environment.NewLine +
                               "\t<param2>\tParam 2 Description" + Environment.NewLine +
                               Environment.NewLine;

            var outputAgregator = new OutputAgregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAgregator)
                )
                .Root(configurator => configurator
                    .RegisterParam("param1", "Param 1 Description", "")
                    .RegisterParam("param2", "Param 2 Description", "")
                )
                .Run(new[] { "" + _longPrefix + "help" });

            Assert.Equal(expectedHelp, outputAgregator.ToString());
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithParametersAndParameterSeries()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost [FLAGS]... [PARAMETERS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "\tFlags:" + Environment.NewLine +                               
                               "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help" + Environment.NewLine +
                               Environment.NewLine +
                               "\tParameters:" + Environment.NewLine +
                               "\t<param1>\tParam 1 Description" + Environment.NewLine +
                               "\t<param2>\tParam 2 Description" + Environment.NewLine +
                               "\t<paramSeries...>\tParam series Description" + Environment.NewLine +
                               Environment.NewLine;

            var outputAgregator = new OutputAgregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAgregator)
                )
                .Root(configurator => configurator
                    .RegisterParam("param1", "Param 1 Description", "")
                    .RegisterParam("param2", "Param 2 Description", "")
                    .RegisterParamSeries("paramSeries", "Param series Description")
                )
                .Run(new[] { "" + _longPrefix + "help" });

            Assert.Equal(expectedHelp, outputAgregator.ToString());
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithParamSeries()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\ttesthost [FLAGS]... [PARAMETERS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "\tFlags:" + Environment.NewLine +                               
                               "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help" + Environment.NewLine +
                               Environment.NewLine +
                               "\tParameters:" + Environment.NewLine +
                               "\t<paramSeries...>\tParam series Description" + Environment.NewLine +
                               Environment.NewLine;

            var outputAgregator = new OutputAgregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAgregator)
                )
                .Root(configurator => configurator
                    .RegisterParamSeries("paramSeries", "Param series Description")
                )
                .Run(new[] { "" + _longPrefix + "help" });

            Assert.Equal(expectedHelp, outputAgregator.ToString());
        }

        [Fact]
        public void PrintHelp_ShouldPrintBasicInfoWithNameDescriptionFlagsOptionsAndParams()
        {
            var expectedHelp = "Usage:" + Environment.NewLine +
                               "\tcustName [FLAGS]... [OPTIONS]... [PARAMETERS]..." + Environment.NewLine +
                               Environment.NewLine +
                               "some description" + Environment.NewLine +
                               Environment.NewLine +
                               "\tFlags:" + Environment.NewLine +
                               "\t" + _longPrefix + "flag1\tFlag 1 Description" + Environment.NewLine +
                               "\t" + _longPrefix + "flag2, " + _shortPrefix + "f2\tFlag 2 Description" + Environment.NewLine +
                               "\t" + _longPrefix + "help, " + _shortPrefix + "h\tPrints this help" + Environment.NewLine +
                               Environment.NewLine +
                               "\tOptions:" + Environment.NewLine +
                               "\t" + _longPrefix + "option1" + _optionValueSeparator + "<option1>\tOption 1 Description" + Environment.NewLine +
                               "\t" + _longPrefix + "option2" + _optionValueSeparator + "<option2>, " + _shortPrefix + "o2" + _optionValueSeparator + "<option2>\tOption 2 Description" + Environment.NewLine +
                               Environment.NewLine +
                               "\tParameters:" + Environment.NewLine +
                               "\t<param1>\tParam 1 Description" + Environment.NewLine +
                               "\t<param2>\tParam 2 Description" + Environment.NewLine +
                               Environment.NewLine;

            var outputAgregator = new OutputAgregator();
            NFlags.Configure(configurator => configurator
                    .SetDialect(_dialect)
                    .SetOutput(outputAgregator)
                    .SetName("custName")
                    .SetDescription("some description")
                )
                .Root(configurator => configurator
                    .RegisterFlag("flag1", "Flag 1 Description", false)
                    .RegisterFlag("flag2", "f2", "Flag 2 Description", false)
                    .RegisterOption("option1", "Option 1 Description", "")
                    .RegisterOption("option2", "o2", "Option 2 Description", "")
                    .RegisterParam("param1", "Param 1 Description", "")
                    .RegisterParam("param2", "Param 2 Description", "")
                )
                .Run(new[] { "" + _longPrefix + "help" });

            Assert.Equal(expectedHelp, outputAgregator.ToString());
        }
    }
}