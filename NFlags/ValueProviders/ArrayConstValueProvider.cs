using System;
using System.Collections.Generic;
using System.Linq;

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

        public T ReadValue<T>()
        {
            var elementType = typeof(T).GetElementType();

            return (T)Convert.ChangeType(_value.Select(input => Convert.ChangeType(input, elementType)).ToArray(), typeof(T));
        }
    }
}