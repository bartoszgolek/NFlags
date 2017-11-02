using System;

namespace NFlags 
{
    /// <summary>
    /// Represents registered application parameter.
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// Parameter name for help printing.
        /// </summary>
        public string Name;

        /// <summary>
        /// Parameter description for help printing.
        /// </summary>
        public string Description;

        /// <summary>
        /// Acion to handle parmeter when found.
        /// </summary>
        public Action<string> Action;
    }
}