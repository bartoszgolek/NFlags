using System.Collections.Generic;
using System.Linq;
using NFlags.ValueProviders;

namespace NFlags.Commands
{
    internal class ValueProvidersCollection
    {
        readonly IList<IValueProvider> _valueProviders = new List<IValueProvider>();

        public void RegisterValueProvider(IValueProvider valueProvider)
        {
            _valueProviders.Insert(0, valueProvider);
        }

        public T GetValue<T>()
        {
            return (T)_valueProviders
                .Select(valueProvider => valueProvider.ReadValue<T>())
                .FirstOrDefault(value => value != null);
        }
    }
}