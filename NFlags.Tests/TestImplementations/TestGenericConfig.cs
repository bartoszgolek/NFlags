using System.Collections.Generic;

namespace NFlags.Tests.TestImplementations
{
    public class TestGenericConfig : IGenericConfig
    {
        private readonly IDictionary<string, object> _variables = new Dictionary<string, object>();

        public TestGenericConfig SetConfigValue<T>(string name, T value)
        {
            _variables[name] = value;

            return this;
        }

        public bool Has(string path)
        {
            return _variables.ContainsKey(path);
        }

        public T Get<T>(string name)
        {
            return _variables.ContainsKey(name) ? (T)_variables[name] : default;
        }
    }
}