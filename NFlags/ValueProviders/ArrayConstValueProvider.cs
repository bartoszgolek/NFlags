using System.Collections.Generic;

namespace NFlags.ValueProviders
{
    internal class ArrayConstValueProvider : IValueProvider
    {
        private readonly List<object> _value = new List<object>();

        /// <inheritdoc />
        public ArrayConstValueProvider(object value)
        {
            _value.Add(value);
        }

        public void Add(object value)
        {
            _value.Add(value);
        }

        public object readValue()
        {
            return _value.ToArray();
        }
    }
}