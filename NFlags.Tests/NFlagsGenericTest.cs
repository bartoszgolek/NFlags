using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Adapter;
using NFlags.GenericCommandExtension;
using NFlags.Tests.DataTypes;
using Xunit;

namespace NFlags.Tests
{
    public class GenericCommandsTest
    {
        [Fact]
        public void TestNFlags_RegisterCommandT_Exists()
        {
            NFlags
                .Configure(c => c.SetDialect(Dialect.Gnu))
                .Root<ArgumentsType>(c => { })
                .Run(new[] {"--help"});
        }
    }
}