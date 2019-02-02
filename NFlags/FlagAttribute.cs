using System;

namespace NFlags
{
    /// <inheritdoc />
    /// <summary>
    /// Mark field or property as flag to register in command
    /// </summary>
    /// <remarks>If property, then has to be settable.</remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FlagAttribute : Attribute
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of FlagAttribute
        /// </summary>
        public FlagAttribute()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of FlagAttribute
        /// </summary>
        /// <param name="name">Name of flag</param>
        /// <param name="abr">Abbreviation of flag</param>
        /// <param name="description">Description of flag for help.</param>
        /// <param name="defaultValue">Default value if flag is not passed trough arguments.</param>
        public FlagAttribute(string name, string abr, string description, bool defaultValue)
        {
            Name = name;
            Abr = abr;
            Description = description;
            DefaultValue = defaultValue;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of FlagAttribute
        /// </summary>
        /// <param name="name">Name of flag</param>
        /// <param name="description">Description of flag for help</param>
        /// <param name="defaultValue">Default value if flag is not passed trough arguments</param>
        public FlagAttribute(string name, string description, bool defaultValue)
        {
            Name = name;
            Description = description;
            DefaultValue = defaultValue;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of FlagAttribute
        /// </summary>
        /// <param name="name">Name of flag</param>
        /// <param name="abr">Abbreviation of flag</param>
        /// <param name="description">Description of flag for help.</param>
        /// <param name="environmentVariable">Name of environment variable to use before defaultValue when argument is not passed.</param>
        /// <param name="defaultValue">Default value if flag is not passed trough arguments.</param>
        public FlagAttribute(string name, string abr, string description, string environmentVariable, bool defaultValue)
        {
            Name = name;
            Abr = abr;
            Description = description;
            EnvironmentVariable = environmentVariable;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Name of flag
        /// </summary>
        public string Name { get;  set; }

        /// <summary>
        /// Abbreviation of flag
        /// </summary>
        public string Abr { get;  set; }

        /// <summary>
        /// Description of flag for help
        /// </summary>
        public string Description { get;  set; }

        /// <summary>
        /// Name of environment variable to use before defaultValue when argument is not passed.
        /// </summary>
        public string EnvironmentVariable { get;  set; }

        /// <summary>
        /// Default value if flag is not passed trough arguments
        /// </summary>
        public bool DefaultValue { get;  set; }
    }
}