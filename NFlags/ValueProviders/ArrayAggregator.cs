using System;
using System.Collections.Generic;
using System.Linq;
using NFlags.Utils;

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
            return ArrayUtils.GetArray(_value.ToArray(), arrayType);
        }
    }
}