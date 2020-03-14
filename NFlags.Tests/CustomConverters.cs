using NFlags.Commands;
using NFlags.Tests.DataTypes;
using NFlags.TypeConverters;
using Xunit;

namespace NFlags.Tests
{
    public class CustomConverters
    {
        [Fact]
        public void TestParam_ShouldUseCustomConverterForParameter_IfRegistered()
        {
            CommandArgs args = null;
            Cli
                .Configure(c => c.RegisterConverter(new CustomTypeConverter()))
                .Root(c => c
                    .RegisterParameter<CustomType>("custom", "CustomType", null)
                    .SetExecute((commandArgs, output) => { args = commandArgs; })
                )
                .Run(new []{"x"});

            Assert.Equal("x", args.GetParameter<CustomType>("custom").SomeString);
        }

        [Fact]
        public void TestParam_ShouldUseCustomConverterConfiguredForParameter_IfSet()
        {
            CommandArgs args = null;
            Cli
                .Configure(c => {})
                .Root(c => c
                    .RegisterParameter<CustomType>(b => b
                        .Name("custom")
                        .Description("CustomType")
                        .Converter(new CustomTypeConverter())
                    )
                    .SetExecute((commandArgs, output) => { args = commandArgs; })
                )
                .Run(new []{"x"});

            Assert.Equal("x", args.GetParameter<CustomType>("custom").SomeString);
        }

        [Fact]
        public void TestParam_ShouldThrowExceptionDuringParameterRegistration_IfConverterIsNotRegistered()
        {
            Assert.Throws<MissingConverterException>(() =>
            {
                Cli
                    .Configure(c => { })
                    .Root(c => c
                        .RegisterParameter<CustomType>("custom", "CustomType", null)
                    );
            });
        }

        [Fact]
        public void TestParam_ShouldThrowExceptionDuringParameterRegistration_IfConverterCantConvertValue()
        {
            Assert.Throws<MissingConverterException>(() =>
            {
                Cli
                    .Configure(c => { })
                    .Root(c => c
                        .RegisterParameter<UnsupportedCustomType>(b => b
                            .Name("custom")
                            .Converter(new CommonTypeConverter())
                        )
                    );
            });
        }

        [Fact]
        public void TestParam_ShouldUseCustomConverterForParameterSeries_IfRegistered()
        {
            CommandArgs args = null;
            Cli
                .Configure(c => c.RegisterConverter(new CustomTypeConverter()))
                .Root(c => c
                    .RegisterParameterSeries<CustomType>("custom", "CustomType")
                    .SetExecute((commandArgs, output) => { args = commandArgs; })
                )
                .Run(new []{"x"});

            Assert.Equal("x", args.GetParameterFromSeries<CustomType>(0).SomeString);
        }

        [Fact]
        public void TestParam_ShouldUseCustomConverterConfiguredForParameterSeries_IfSet()
        {
            CommandArgs args = null;
            Cli
                .Configure(c => {})
                .Root(c => c
                    .RegisterParameterSeries<CustomType>(b => b
                        .Name("custom")
                        .Description("CustomType")
                        .Converter(new CustomTypeConverter())
                    )
                    .SetExecute((commandArgs, output) => { args = commandArgs; })
                )
                .Run(new []{"x"});

            Assert.Equal("x", args.GetParameterFromSeries<CustomType>(0).SomeString);
        }

        [Fact]
        public void TestParam_ShouldThrowExceptionDuringParameterSeriesRegistration_IfConverterIsNotRegistered()
        {
            Assert.Throws<MissingConverterException>(() =>
            {
                Cli
                    .Configure(c => { })
                    .Root(c => c
                        .RegisterParameterSeries<CustomType>("custom", "CustomType")
                    );
            });
        }

        [Fact]
        public void TestParam_ShouldThrowExceptionDuringParameterSeriesRegistration_IfConverterCantConvertValue()
        {
            Assert.Throws<MissingConverterException>(() =>
            {
                Cli
                    .Configure(c => { })
                    .Root(c => c
                        .RegisterParameterSeries<UnsupportedCustomType>(b => b
                            .Name("custom")
                            .Converter(new CommonTypeConverter())
                        )
                    );
            });
        }

        [Fact]
        public void TestOption_ShouldUseCustomConverterForOption_IfRegistered()
        {
            CommandArgs args = null;
            Cli
                .Configure(c => c.RegisterConverter(new CustomTypeConverter()))
                .Root(c => c
                    .RegisterOption<CustomType>("custom", "CustomType", null)
                    .SetExecute((commandArgs, output) => { args = commandArgs; })
                )
                .Run(new []{"/custom=x"});

            Assert.Equal("x", args.GetOption<CustomType>("custom").SomeString);
        }

        [Fact]
        public void TestOption_ShouldUseCustomConverterConfiguredForOption_IfSet()
        {
            CommandArgs args = null;
            Cli
                .Configure(c => { })
                .Root(c => c
                    .RegisterOption<CustomType>(b => b
                            .Name("custom")
                            .Description("CustomType")
                            .DefaultValue(null)
                            .Converter(new CustomTypeConverter())
                    )
                    .SetExecute((commandArgs, output) => { args = commandArgs; })
                )
                .Run(new []{"/custom=x"});

            Assert.Equal("x", args.GetOption<CustomType>("custom").SomeString);
        }

        [Fact]
        public void TestOption_ShouldThrowExceptionDuringOptionRegistration_IfConverterIsNotRegistered()
        {
            Assert.Throws<MissingConverterException>(() =>
            {
                Cli
                    .Configure(c => { })
                    .Root(c => c
                        .RegisterOption<CustomType>("custom", "CustomType", null)
                    );
            });
        }

        [Fact]
        public void TestOption_ShouldThrowExceptionDuringOptionRegistration_IfOptionConverterCantConvertValue()
        {
            Assert.Throws<MissingConverterException>(() =>
            {
                Cli
                    .Configure(c => { })
                    .Root(c => c
                        .RegisterOption<UnsupportedCustomType>(b => b
                            .Name("custom")
                            .Converter(new CustomTypeConverter())
                        )
                    );
            });
        }
    }
}