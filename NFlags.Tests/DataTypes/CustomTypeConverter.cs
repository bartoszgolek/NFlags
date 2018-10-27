using System;
using NFlags.TypeConverters;

namespace NFlags.Tests.DataTypes
{
    public class CustomTypeConverter : IArgumentConverter
    {
        public bool CanConvert(Type type)
        {
            return type == typeof(CustomType);
        }

        public object Convert(Type type, string value)
        {
            return new CustomType
            {
                SomeString = value
            };
        }
    }
}