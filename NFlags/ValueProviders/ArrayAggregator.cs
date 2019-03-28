using System;
using System.Collections.Generic;
using System.Linq;

namespace NFlags.ValueProviders
{
    internal class ArrayAggregator
    {
        private readonly List<object> _value = new List<object>();

        /// <inheritdoc />
        public ArrayAggregator(object value)
        {
            _value.Add(value);
        }

        public void Add(object value)
        {
            _value.Add(value);
        }

        public object GetArray(Type arrayType)
        {
            var elementType = arrayType.GetElementType();
            var instance = Array.CreateInstance(elementType, _value.Count);
            for (var i = 0; i < _value.Count; i++)
            {
                instance.SetValue(_value[i], i);
            }

            return instance;
        }
    }
}