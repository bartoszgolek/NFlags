using System;

namespace NFlags.Utils
{
    internal static class ArrayUtils
    {
        public static object GetArray(object[] values, Type arrayType)
        {
            var elementType = arrayType.GetElementType();
            var instance = Array.CreateInstance(elementType, values.Length);
            for (var i = 0; i < values.Length; i++)
            {
                instance.SetValue(values[i], i);
            }

            return instance;
        }
    }
}