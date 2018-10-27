using System;

namespace NFlags.TypeConverters
{
    /// <summary>
    /// Defines converter to convert parameter to expected type 
    /// </summary>
    public interface IArgumentConverter
    {
        /// <summary>
        /// Determines if converter can convert to type. 
        /// </summary>
        /// <param name="type">Target type</param>
        /// <returns></returns>
        bool CanConvert(Type type);
        
        /// <summary>
        /// Converts value to given type
        /// </summary>
        /// <param name="type">Target type</param>
        /// <param name="value">Value to convert.</param>
        /// <returns></returns>
        object Convert(Type type, string value);
    }
}