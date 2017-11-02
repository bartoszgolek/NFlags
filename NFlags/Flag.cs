using System;

namespace NFlags
{
    /// <summary>
    /// Represents registered application flag.
    /// </summary>
    public class Flag
    {
        /// <summary>
        /// Flag name for help printing.
        /// </summary>
        public string Name;

        /// <summary>
        /// Flag description for help printing.
        /// </summary>
        public string Description;

        /// <summary>
        /// Flag abreviation name for help printing and shorthand usage.
        /// </summary>
        public string Abr;

        /// <summary>
        /// Acion to handle flag when found.
        /// </summary>
        public Action Action;
    }
}