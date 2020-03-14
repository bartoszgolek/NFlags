using System.Linq;
using NFlags.Arguments;
using NFlags.Utils;

namespace NFlags.Commands
{
    internal class ArgumentValueReader
    {
        private readonly ValueConverter _valueConverter;

        public ArgumentValueReader(ValueConverter valueConverter)
        {
            _valueConverter = valueConverter;
        }

        public object Read(Argument argument, string value)
        {
            if (value == null)
                return null;

            if (!argument.ValueType.IsArray)
                return _valueConverter.ConvertValueToExpectedType(value, argument.ValueType, argument.Converter);

            var values = value.Split(';')
                .Select(v => _valueConverter.ConvertValueToExpectedType(v, argument.ValueType.GetElementType(), argument.Converter))
                .ToArray();

            return ArrayUtils.GetArray(
                values,
                argument.ValueType);
        }
    }
}