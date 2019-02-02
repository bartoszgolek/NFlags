namespace NFlags.Utils
{
    /// <inheritdoc />
    public class SystemEnvironment : IEnvironment
    {
        /// <inheritdoc />
        public string Get(string name)
        {
            return System.Environment.GetEnvironmentVariable(name);
        }
    }
}