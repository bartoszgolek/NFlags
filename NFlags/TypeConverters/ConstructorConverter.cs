using System;
using System.Reflection;

namespace NFlags.TypeConverters
{
    /// <inheritdoc />
    /// <summary>
    /// Convert string to type using constructor.
    /// </summary>
    public class ConstructorConverter : IArgumentConverter
    {
        /// <inheritdoc />
        public bool CanConvert(Type type)
        {
            return GetConstructorInfo(type) != null;
        }

        /// <inheritdoc />
        public object Convert(Type type, string value)
        {
            var constructorInfo = GetConstructorInfo(type);
            if (constructorInfo != null)
                return constructorInfo.Invoke(new object[] {value});
            
            throw new ArgumentValueException(type, value);
        }

        private static ConstructorInfo GetConstructorInfo(Type type)
        {
            return type.GetConstructor(new[] {typeof(string)});
        }
    }
}