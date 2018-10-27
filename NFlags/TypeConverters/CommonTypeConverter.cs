using System;
using NFlags.Commands;

namespace NFlags.TypeConverters
{
    /// <summary>
    /// Converter to convert to common language types
    /// </summary>
    public class CommonTypeConverter: IArgumentConverter
    {
        /// <inheritdoc />
        public bool CanConvert(Type type)
        {
            return type == typeof(bool) ||
                   type == typeof(byte) ||
                   type == typeof(char) ||
                   type == typeof(DateTime) ||
                   type == typeof(decimal) ||
                   type == typeof(double) ||
                   type == typeof(short) ||
                   type == typeof(int) ||
                   type == typeof(long) ||
                   type == typeof(sbyte) ||
                   type == typeof(float) ||
                   type == typeof(string) ||
                   type == typeof(ushort) ||
                   type == typeof(uint) ||
                   type == typeof(ulong);
        }

        /// <inheritdoc />
        public object Convert(Type type, string value)
        {
            try
            {
                return System.Convert.ChangeType(value, type);
            }
            catch (FormatException)
            {
                throw new ArgumentValueException(type, value);
            }
        }
    }
}