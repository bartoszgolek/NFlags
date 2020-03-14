using System;
using System.Collections.Generic;
using NFlags.TypeConverters;

namespace NFlags.Commands
{
    internal class ValueConverter
    {
        private readonly IEnumerable<IArgumentConverter> _argumentConverters;

        public ValueConverter(IEnumerable<IArgumentConverter> argumentConverters)
        {
            _argumentConverters = argumentConverters;
        }

        public object ConvertValueToExpectedType(string value, Type expectedType, IArgumentConverter converter)
        {
            if (value == null)
                return null;

            if (expectedType == typeof(string))
                return value;

            if (converter != null && converter.CanConvert(expectedType))
                return converter.Convert(expectedType, value);

            foreach (var c in _argumentConverters)
                if (c.CanConvert(expectedType))
                    return c.Convert(expectedType, value);

            throw new MissingConverterException(expectedType);
        }
    }
}