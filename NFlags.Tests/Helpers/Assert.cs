using System.Text;

namespace NFlags.Tests.Helpers
{
    public class Assert
    {
        public static void HelpEquals(OutputAggregator output, params string[] lines)
        {
            var expectedResultBuilder = new StringBuilder();
            foreach (var line in lines)
            {
                expectedResultBuilder.AppendLine(line);                
            }

            Xunit.Assert.Equal(expectedResultBuilder.ToString(), output.ToString());
        }
    }
}