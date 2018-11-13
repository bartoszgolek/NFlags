using Xunit;
using NFAssert = NFlags.Tests.Helpers.Assert;

namespace NFlags.Tests
{
    public class PrintHelpOnExecute
    {
        [Fact]
        public void TestRun_ShouldPrintHelpWhenCommandExecuted()
        {
            var outputAggregator = new OutputAggregator();
            NFlags
                .Configure(c => c.SetOutput(outputAggregator))
                .Root(c => c.PrintHelpOnExecute())
                .Run(new string[0]);

                NFAssert.HelpEquals(outputAggregator,
                    "Usage:",
                    "\ttesthost [FLAGS]...",
                    "",
                    "\tFlags:",
                    "\t/help, /h\tPrints this help",
                    ""
                );            
        }
        
        [Fact]
        public void TestRun_ShouldThrowMissingCommandImplementationException_IfBothPrintHelpOnExecuteAndSetExecuteAreNotSet()
        {
            Assert.Throws<MissingCommandImplementationException>(() =>
            {
                NFlags
                    .Configure(c => { })
                    .Root(c => { })
                    .Run(new string[0]);
            });         
        }
    }
}