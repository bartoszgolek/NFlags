using System;

namespace NFlags
{
    /// <summary>
    /// Represents registered application option.
    /// </summary>
    public class Option
    {
        /// <summary>
        /// Option name for help printing.
        /// </summary>
        public string Name;

        /// <summary>
        /// Option description for help printing.
        /// </summary>
        public string Description;

        /// <summary>
        /// Option abreviation name for help printing and shorthand usage.
        /// </summary>
        public string Abr;

        /// <summary>
        /// Acion to handle option when found.
        /// </summary>
        public Action<string> Action;
    }
}