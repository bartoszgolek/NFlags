using System;

namespace NFlags.TypeConverters
{
    /// <summary>
    /// Defines converter to convert parameter to expected type
    /// </summary>
    public interface IArgumentConverter
    {
        /// <summary>
        /// Determines if converter can convert to type
        /// </summary>
        /// <param name="type">Target type</param>
        /// <returns>True if converter can convert to given target type, otherwise False</returns>
        bool CanConvert(Type type);

        /// <summary>
        /// Converts value to given target type
        /// </summary>
        /// <param name="type">Target type</param>
        /// <param name="value">Value to convert.</param>
        /// <returns>Converted value</returns>
        object Convert(Type type, string value);
    }
}