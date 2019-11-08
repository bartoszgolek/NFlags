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

        public object ConvertValueToExpectedType(string value, Type expectedType)
        {
            if (value == null)
                return null;

            if (expectedType == typeof(string))
                return value;

            foreach (var converter in _argumentConverters)
                if (converter.CanConvert(expectedType))
                    return converter.Convert(expectedType, value);

            throw new MissingConverterException(expectedType);
        }
    }
}