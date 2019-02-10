using System.Collections.Generic;
using System.Linq;
using NFlags.ValueProviders;

namespace NFlags.Commands
{
    public class ValueProvidersCollection
    {
        IList<IValueProvider> _valueProviders = new List<IValueProvider>();

        public void RegisterValueProvider(IValueProvider valueProvider)
        {
            _valueProviders.Insert(0, valueProvider);
        }

        public object GetValue()
        {
            return _valueProviders
                .Select(valueProvider => valueProvider.readValue())
                .FirstOrDefault(value => value != null);
        }
    }
}