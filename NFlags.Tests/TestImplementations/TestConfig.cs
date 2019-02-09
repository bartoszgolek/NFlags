using System.Collections.Generic;

namespace NFlags.Tests.TestImplementations
{
    public class TestConfig : IConfig
    {
        private readonly IDictionary<string, string> _variables = new Dictionary<string, string>();

        public TestConfig SetConfigValue(string name, string value)
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