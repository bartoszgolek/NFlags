using NFlags.Commands;
using NFlags.Tests.DataTypes;
using NFlags.TypeConverters;
using Xunit;

namespace NFlags.Tests
{
    public class CustomConverters
    {
        [Fact]
        public void TestUseCustomConverterForParameter_IfRegistered()
        {
            CommandArgs args = null;
            NFlags
                .Configure(c => c.RegisterConverter(new CustomTypeConverter()))
                .Root(c => c
                    .RegisterParam<CustomType>("custom", "CustomType", null)
                    .SetExecute((commandArgs, output) => { args = commandArgs; })
                )
                .Run(new []{"x"});

            Assert.IsType<CustomType>(args.Parameters["custom"]);
            Assert.Equal("x", ((CustomType)args.Parameters["custom"]).SomeString);
        }

        [Fact]
        public void TestThrowExceptionDuringParameterRegistration_IfConverterIsNotRegistered()
        {
            Assert.Throws<MissingConverterException>(() =>
            {
                NFlags
                    .Configure(c => { })
                    .Root(c => c
                        .RegisterParam<CustomType>("custom", "CustomType", null)
                    );
            });
        }
        
        [Fact]
        public void TestUseCustomConverterForParameterSeries_IfRegistered()
        {
            CommandArgs args = null;
            NFlags
                .Configure(c => c.RegisterConverter(new CustomTypeConverter()))
                .Root(c => c
                    .RegisterParameterSeries<CustomType>("custom", "CustomType")
                    .SetExecute((commandArgs, output) => { args = commandArgs; })
                )
                .Run(new []{"x"});

            Assert.IsType<CustomType>(args.ParameterSeries[0]);
            Assert.Equal("x", ((CustomType)args.ParameterSeries[0]).SomeString);
        }

        [Fact]
        public void TestThrowExceptionDuringParameterSeriesRegistration_IfConverterIsNotRegistered()
        {
            Assert.Throws<MissingConverterException>(() =>
            {
                NFlags
                    .Configure(c => { })
                    .Root(c => c
                        .RegisterParameterSeries<CustomType>("custom", "CustomType")
                    );
            });
        }
        
        [Fact]
        public void TestUseCustomConverterForOption_IfRegistered()
        {
            CommandArgs args = null;
            NFlags
                .Configure(c => c.RegisterConverter(new CustomTypeConverter()))
                .Root(c => c
                    .RegisterOption<CustomType>("custom", "CustomType", null)
                    .SetExecute((commandArgs, output) => { args = commandArgs; })
                )
                .Run(new []{"/custom=x"});

            Assert.IsType<CustomType>(args.Options["custom"]);
            Assert.Equal("x", ((CustomType)args.Options["custom"]).SomeString);
        }

        [Fact]
        public void TestThrowExceptionDuringOptionRegistration_IfConverterIsNotRegistered()
        {
            Assert.Throws<MissingConverterException>(() =>
            {
                NFlags
                    .Configure(c => { })
                    .Root(c => c
                        .RegisterOption<CustomType>("custom", "CustomType", null)
                    );
            });
        }
    }
}