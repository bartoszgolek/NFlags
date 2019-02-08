using System.Collections.Generic;

namespace NFlags.Tests.TestImplementations
{
    public class TestEnvironment : IEnvironment
    {
        private readonly IDictionary<string, string> _variables = new Dictionary<string, string>();

        public TestEnvironment SetEnvironmentVariable(string name, string value)
        {
            _variables[name] = value;

            return this;
        }

        public string Get(string name)
        {
            return _variables.ContainsKey(name) ? _variables[name] : null;
        }
    }
}